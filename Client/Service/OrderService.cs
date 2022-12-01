using ApiServerForTelegram.Entities.EExceptions;
using ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using TlgWebAppNet;
using WebAppAssembly.Shared.Entities.CreateDelivery;
using WebAppAssembly.Shared.Entities.Exceptions;
using WebAppAssembly.Shared.Entities.IikoCloudApi;
using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.Entities.WebApp;
using WebAppAssembly.Shared.Models.Order;
using OrderControllerPathsOfClientSide = WebAppAssembly.Shared.Entities.WebApp.OrderControllerPaths;

namespace WebAppAssembly.Client.Service
{
    public class OrderService : IOrderService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="urlPathOfMainInfo"></param>
        /// <exception cref="InfoException"></exception>
        public OrderService(HttpClient client, long chatId, string urlPathOfMainInfo)
        {
            Http = client;

            var mainInfoTask = RetrieveMainInfoForWebAppOrderAsync(client, chatId, urlPathOfMainInfo);
            mainInfoTask.Wait();
            var mainInfo = mainInfoTask.Result;

            OrderInfo = OrderInfoInit(mainInfo.OrderInfo);
            if (OrderInfo.ChatId == 0) OrderInfo.ChatId = chatId;
            OrderInfo.Address = new DeliveryPoint();
            DeliveryGeneralInfo = mainInfo.DeliveryGeneralInfo ?? throw new InfoException(typeof(OrderService).FullName!,
                nameof(Exception), typeof(DeliveryGeneralInfo).FullName!, ExceptionType.Null);
            IsDiscountBalanceConfirmed = false;
            IsReleaseMode = mainInfo.IsReleaseMode;
            var tlgMainBtnClr = !string.IsNullOrEmpty(mainInfo.TlgMainBtnColor)
                ? mainInfo.TlgMainBtnColor : mainInfo.TlgMainBtnColor ?? throw new InfoException(typeof(OrderService).FullName!,
                nameof(Exception), $"{typeof(MainInfoForWebAppOrder).FullName!}.{nameof(MainInfoForWebAppOrder.TlgMainBtnColor)}", ExceptionType.NullOrEmpty);
            TlgMainBtnColor = IsRgbColorFormat(tlgMainBtnClr) ? mainInfo.TlgMainBtnColor : throw new InfoException(typeof(OrderService).FullName!,
                nameof(Exception), $"Incorrect formant of rgb (#rrggbb) color for the main button of the Telegram. Current value is '{tlgMainBtnClr}'");
            HaveSelectedItemsInOrder = OrderInfo.HaveSelectedProducts();
        }

        private HttpClient Http { get; set; }
        public OrderModel OrderInfo { get; set; }
        public DeliveryGeneralInfo DeliveryGeneralInfo { get; set; }
        public bool IsDiscountBalanceConfirmed { get; set; }
        public CurrentProduct? CurrentProduct { get; set; }
        public Item? CurrItem { get; set; }
        public TransportItemDto? CurrProductItem { get; set; }
        public Guid? CurrentGroupId { get; set; }
        public bool IsReleaseMode { get; set; }
        public string TlgMainBtnColor { get; set; }
        private bool HaveSelectedItemsInOrder { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="urlPathOfMainInfo"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        private static async Task<MainInfoForWebAppOrder> RetrieveMainInfoForWebAppOrderAsync(HttpClient client, long chatId, string urlPathOfMainInfo)
        {
            var chatInfo = new ChatInfo() { ChatId = chatId };
            var body = JsonConvert.SerializeObject(chatInfo);
            var data = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(urlPathOfMainInfo, data);

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.StatusCode.Equals(HttpStatusCode.OK))
                throw new HttpProcessException(response.StatusCode, responseBody);

            return JsonConvert.DeserializeObject<MainInfoForWebAppOrder>(responseBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private static bool IsRgbColorFormat(string color)
        {
            const int rgbFormatLength = 7;
            string clrCopy = new(color);

            if (clrCopy.Length != rgbFormatLength && !clrCopy.StartsWith('#'))
                return false;

            clrCopy = clrCopy.Remove(0);
            foreach (var chr in clrCopy)
                if (!char.IsDigit(chr)) return false;

            return true;
        }

        /// <summary>
        /// !!! Redone it for better !!!
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        private OrderModel OrderInfoInit(OrderModel? orderInfo = null)
        {
            if (orderInfo is not null)
            {
                orderInfo.Items ??= new List<Item>();

                if (!orderInfo.Items.Any())
                    orderInfo.WithNewParameters();
                else
                {
                    var items = orderInfo.Items;
                    var products = DeliveryGeneralInfo.TransportItemDtos;
                    if (products is not null)
                        foreach (var item in items) products.First(y => y.ItemId == item.ProductId).TotalAmount += (int)item.Amount;
                }
                orderInfo.FreeItems ??= new List<Item>();
                orderInfo.DiscountFreeItems ??= new List<Guid>();
                return orderInfo;
            }
            else return new OrderModel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<ProductInfo> AddProductItemInSelectingProductPageAsync(Guid productId)
        {
            var product = DeliveryGeneralInfo.ProductById(productId, CurrentGroupId);
            var items = OrderInfo.CurrItems();

            if (product.HaveModifiersOrSizesMoreThanOne())
            {
                var positionId = AddItemToOrderWithNewPosition(product);
                var item = items.Last(x => x.ProductId == productId && x.PositionId == positionId);
                if (product.HaveSizesMoreThanOne()) item.ProductSizeId = product.ItemSizes?.FirstOrDefault()?.SizeId;
                CurrentProduct = new CurrentProduct(productId, positionId);
                CurrItem = item;
                CurrProductItem = product;
                IncreaseAmountOfProduct(product, item);
                return new(product, item, !HaveSelectedProductsAtFirst());
            }
            else
            {
                var item = items.FirstOrDefault(x => x.ProductId == productId);
                if (item is null)
                {
                    AddItemToOrderWithNewPosition(product);
                    item = items.Last(x => x.ProductId == productId);
                }
                IncreaseAmountOfProduct(product, item);
                await SendChangedOrderModelToServerAsync();
                return new(product, item, !HaveSelectedProductsAtFirst());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<ProductInfo?> RemoveProductItemInSelectingProductPageAsync(Guid productId)
        {
            var product = DeliveryGeneralInfo.ProductById(productId, CurrentGroupId);
            var item = OrderInfo.ItemByIdOrDefault(productId);
            if (item is null) return null;

            bool isRemoved;
            if (isRemoved = RemoveOrDecreaseAmountOfProduct(product, item))
                HaveSelectedItemsInOrder = OrderInfo.HaveSelectedProducts();

            await SendChangedOrderModelToServerAsync();
            return isRemoved ? null : new(product, item, !HaveSelectedProductsAtFirst());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<ProductInfo?> RemoveProductItemInSelectingProductPageAsync(TransportItemDto product)
        {
            var item = OrderInfo.ItemByIdOrDefault((Guid)product.ItemId!);
            if (item is null) return null;

            bool isRemoved;
            if (isRemoved = RemoveOrDecreaseAmountOfProduct(product, item))
                HaveSelectedItemsInOrder = OrderInfo.HaveSelectedProducts();

            await SendChangedOrderModelToServerAsync();
            return isRemoved ? null : new(product, item, !HaveSelectedProductsAtFirst());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public async Task<ProductInfo> AddProductItemInShoppingCartPageAsync(ITwaNet twaNet, Guid productId, Guid? positionId = null)
        {
            var product = DeliveryGeneralInfo.ProductById(productId, CurrentGroupId);
            var item = OrderInfo.ItemById(productId, positionId);

            IncreaseAmountOfProduct(product, item);

            if (DeliveryGeneralInfo.UseIikoBizProgram)
                await CalculateLoayltyProgramAndAllowedSumAsync(twaNet);

            // !!! Save changed the order info by using 'CalculateLoyaltyProgramAsync' method !!!
            await SendChangedOrderModelToServerAsync();
            return new(product, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public async Task<ProductInfo> AddProductItemInShoppingCartPageAsync(Guid productId, Guid? positionId = null)
        {
            var product = DeliveryGeneralInfo.ProductById(productId, CurrentGroupId);
            var item = OrderInfo.ItemById(productId, positionId);

            IncreaseAmountOfProduct(product, item);

            if (DeliveryGeneralInfo.UseIikoBizProgram)
                await CalculateLoayltyProgramAndAllowedSumAsync();

            // !!! Save changed the order info by using 'CalculateLoyaltyProgramAsync' method !!!
            await SendChangedOrderModelToServerAsync();
            return new(product, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public async Task<ProductInfo?> RemoveProductItemInShoppingCartPageAsync(ITwaNet twaNet, Guid productId, Guid? positionId = null)
        {
            var product = DeliveryGeneralInfo.ProductById(productId, CurrentGroupId);
            var item = OrderInfo.ItemById(productId, positionId);

            if (RemoveOrDecreaseAmountOfProduct(product, item))
                HaveSelectedItemsInOrder = OrderInfo.HaveSelectedProducts();

            if (HaveSelectedItemsInOrder && DeliveryGeneralInfo.UseIikoBizProgram)
                await CalculateLoayltyProgramAndAllowedSumAsync(twaNet);

            // !!! Save changed the order info by using 'CalculateLoyaltyProgramAsync' method !!!
            await SendChangedOrderModelToServerAsync();
            if (item is null) return null;
            return new(product, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public async Task<ProductInfo?> RemoveProductItemInShoppingCartPageAsync(Guid productId, Guid? positionId = null)
        {
            var product = DeliveryGeneralInfo.ProductById(productId, CurrentGroupId);
            var item = OrderInfo.ItemById(productId, positionId);

            if (RemoveOrDecreaseAmountOfProduct(product, item))
                HaveSelectedItemsInOrder = OrderInfo.HaveSelectedProducts();

            if (HaveSelectedItemsInOrder && DeliveryGeneralInfo.UseIikoBizProgram)
                await CalculateLoayltyProgramAndAllowedSumAsync();

            // !!! Save changed the order info by using 'CalculateLoyaltyProgramAsync' method !!!
            await SendChangedOrderModelToServerAsync();
            if (item is null) return null;
            return new(product, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveProductItemWithModifiersInSelectingProductPageAsync(Guid productId)
        {
            var product = DeliveryGeneralInfo.ProductById(productId, CurrentGroupId);
            var item = OrderInfo.ItemById(productId);

            if (OrderInfo.HaveMoreThanOneItemPositionOfProduct(productId))
            {
                CurrentProduct = new(productId);
                CurrItem = item;
                CurrProductItem = product;
                return true;
            }

            RemoveOrDecreaseAmountOfProduct(product, item);
            await SendChangedOrderModelToServerAsync();
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<bool> RemoveProductItemWithModifiersInSelectingProductPageAsync(TransportItemDto product)
        {
            var item = OrderInfo.ItemById((Guid)product.ItemId!);

            if (OrderInfo.HaveMoreThanOneItemPositionOfProduct((Guid)product.ItemId!))
            {
                CurrentProduct = new((Guid)product.ItemId);
                CurrItem = item;
                CurrProductItem = product;
                return true;
            }

            RemoveOrDecreaseAmountOfProduct(product, item);
            await SendChangedOrderModelToServerAsync();
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public async Task<ProductInfo> AddProductItemInChangingSelectedProductsWithModifiersPageAsync(Guid productId, Guid positionId)
        {
            var product = DeliveryGeneralInfo.ProductById(productId, CurrentGroupId);
            var item = OrderInfo.ItemById(productId, positionId);
            IncreaseAmountOfProduct(product, item);
            await SendChangedOrderModelToServerAsync();
            return new(product, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public async Task<ProductInfo?> RemoveProductItemInChangingSelectedProductsWithModifiersPageAsync(Guid productId, Guid positionId)
        {
            var product = DeliveryGeneralInfo.ProductById(productId, CurrentGroupId);
            var item = OrderInfo.ItemById(productId, positionId);
            var isRemoved = RemoveOrDecreaseAmountOfProduct(product, item);
            await SendChangedOrderModelToServerAsync();
            return isRemoved ? null : new(product, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="modifierId"></param>
        /// <param name="positionId"></param>
        /// <param name="modifierGroupId"></param>
        public Item AddModifierInSelectingModifiersAndAmountsForProductPageAsync(Guid modifierId, Guid? modifierGroupId = null)
        {
            var item = GetCurrItem();
            OrderInfo.IncrementTotalAmountOfModifierWithPrice(item, modifierId, modifierGroupId);
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="modifierId"></param>
        /// <param name="positionId"></param>
        /// <param name="modifierGroupId"></param>
        /// <returns></returns>
        public Item RemoveModifierInSelectingModifiersAndAmountsForProductPageAsync(Guid modifierId, Guid? modifierGroupId = null)
        {
            var item = GetCurrItem();
            OrderInfo.DecrementTotalAmountOfModifierWithPrice(item, modifierId, modifierGroupId);
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public Item AddProductInSelectingModifiersAndAmountsForProductPageAsync()
        {
            var item = GetCurrItem();
            var product = GetCurrProductItem();
            IncreaseAmountOfProduct(product, item);
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public Item? RemoveProductInSelectingModifiersAndAmountsForProductPageAsync()
        {
            var item = GetCurrItem();
            var product = GetCurrProductItem();
            RemoveOrDecreaseAmountOfProduct(product, item);
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public ProductInfo AddProductWithoutModifiersForSelectingAmountsForProductsPageAsync(Guid productId)
        {
            var product = DeliveryGeneralInfo.ProductById(productId, CurrentGroupId);
            if (product.HaveModifiersOrSizesMoreThanOne()) throw new InfoException(typeof(OrderService).FullName!,
                nameof(AddProductWithoutModifiersForSelectingAmountsForProductsPageAsync), nameof(Exception),
                "Adding the product with modifiers for this page is not allowed");

            var item = OrderInfo.ItemByIdOrDefault(productId);
            if (item is null)
            {
                AddItemToOrderWithNewPosition(product);
                item = OrderInfo.ItemById(productId);
            }

            IncreaseAmountOfProduct(product, item);
            return new(product, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProductInfo AddProductWithoutModifiersInSelectingAmountsForProductsPageAsync()
        {
            var product = GetCurrProductItem();
            var item = GetCurrItem();
            IncreaseAmountOfProduct(product, item);
            return new(product, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProductInfo? RemoveProductWithoutModifiersInSelectingAmountsForProductsPageAsync()
        {
            var product = GetCurrProductItem();
            var item = GetCurrItem();
            if (RemoveOrDecreaseAmountOfProduct(product, item))
            {
                HaveSelectedProductsAtFirst();
                return null;
            }
            return new(product, item, !HaveSelectedProductsAtFirst());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private Guid AddItemToOrderWithNewPosition(TransportItemDto product)
        {
            var newId = Guid.NewGuid();
            OrderInfo.Items!.Add(new Item(product, newId));
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="item"></param>
        private void IncreaseAmountOfProduct(TransportItemDto product, Item item)
        {
            OrderInfo.IncrementTotalAmountWithPrice(item);
            product.IncrementAmount(); // ??? Remove it ???
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="item"></param>
        public void DecreaseAmountOfProduct(TransportItemDto product, Item item)
        {
            OrderInfo.DecrementTotalAmountWithPrice(item);
            product.DecrementAmount(); // ??? Remove it ???
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="item"></param>
        public bool RemoveOrDecreaseAmountOfProduct(TransportItemDto product, Item item)
        {
            DecreaseAmountOfProduct(product, item);
            if (!item.HaveItems())
            {
                OrderInfo.ZeroAmountOfItem(item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task SendChangedOrderModelToServerAsync()
        {
            try
            {
                var body = JsonConvert.SerializeObject(OrderInfo);
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await Http.PostAsync(OrderControllerPathsOfClientSide.SendChangedOrderModelToServer, data);

                string responseBody = await response.Content.ReadAsStringAsync();
                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                    throw new HttpProcessException(response.StatusCode, responseBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HaveSelectedProductsAtFirst()
        {
            bool val = HaveSelectedItemsInOrder;
            HaveSelectedItemsInOrder = OrderInfo.HaveSelectedProducts();
            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="HttpProcessException"></exception>
        private async Task<LoyaltyCheckinInfo> CalculateCheckinAsync()
        {
            var json = JsonConvert.SerializeObject(OrderInfo);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Http.PostAsync(OrderControllerPathsOfClientSide.CalculateCheckin, data);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.StatusCode.Equals(HttpStatusCode.OK))
                throw new HttpProcessException(response.StatusCode, responseBody);
            return JsonConvert.DeserializeObject<LoyaltyCheckinInfo>(responseBody) ?? throw new Exception("Json convert order model is empty");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task CalculateLoayltyProgramAndAllowedSumAsync()
        {
            var res = await CalculateLoyaltyProgramAsync();
            if (res.Checkin?.AvailablePayments is not null)
                CalculateAllowedWalletSum(res.Checkin.AvailablePayments);
            else
                ResetWallet();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        public async Task CalculateLoayltyProgramAndAllowedSumAsync(ITwaNet twaNet)
        {
            var res = await CalculateLoyaltyProgramAsync(twaNet);
            if (res.Checkin?.AvailablePayments is not null)
                CalculateAllowedWalletSum(res.Checkin.AvailablePayments);
            else
                ResetWallet();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        private async Task<LoyaltyCheckinInfo> CalculateLoyaltyProgramAsync(ITwaNet twaNet)
        {
            ResetVariablesOfLoyaltyProgram();

            var checkinResult = await CalculateCheckinAsync();

            if (!checkinResult.Ok || checkinResult.Checkin is null)
            {
                Console.WriteLine(checkinResult.HttpResponseInfo?.Message);

                // !!! Need to add check of error message !!!
                if (IsReleaseMode)
                {
                    var popupMessage = DeliveryGeneralInfo.GetTlgWebAppPopupMessages().LoayltyProgramUnavailable ?? throw new InfoException(typeof(OrderService).FullName!,
                        nameof(CalculateLoyaltyProgramAsync), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}.{nameof(TlgWebAppPopupMessages.LoayltyProgramUnavailable)}",
                        ExceptionType.Null);
                    await twaNet.HideProgressAsync();
                    await twaNet.ShowOkPopupMessageAsync(popupMessage.Title, popupMessage.Description, HapticFeedBackNotificationType.warning);
                }
                return checkinResult;
            }

            var checkin = checkinResult.Checkin;

            if (!string.IsNullOrEmpty(checkin.WarningMessage))
            {
                Console.WriteLine(checkin.WarningMessage);

                if (IsReleaseMode)
                {
                    await twaNet.HideProgressAsync();
                    await twaNet.ShowOkPopupMessageAsync(string.Empty, checkin.WarningMessage, HapticFeedBackNotificationType.warning);
                }
                return checkinResult;
            }
            else if (checkin.LoyaltyProgramResults is not null)
            {
                var products = DeliveryGeneralInfo.Products();
                double discountSum = 0;
                var discountFreeItems = new List<Guid>();
                var _freeItems = new List<Item>();

                foreach (var loyaltyProgram in checkin.LoyaltyProgramResults)
                {
                    if (loyaltyProgram.Discounts is not null)
                    {
                        foreach (var discount in loyaltyProgram.Discounts)
                        {
                            discountSum += discount.DiscountSum;
                            if (discount.Code == (int)DiscountType.FreeProduct && discount.OrderItemId is not null && discount.OrderItemId != Guid.Empty)
                                discountFreeItems.Add((Guid)discount.OrderItemId);
                        }
                    }
                    if (loyaltyProgram.FreeProducts is not null)
                    {
                        foreach (var freeProduct in loyaltyProgram.FreeProducts)
                        {
                            if (freeProduct.Products is not null)
                            {
                                foreach (var product in freeProduct.Products)
                                {
                                    var sourceProduct = products.FirstOrDefault(x => x.ItemId == product.Id);
                                    _freeItems.Add(new Item(
                                        productId: product.Id,
                                        productName: sourceProduct?.Name ?? string.Empty,
                                        amount: 1,
                                        type: !string.IsNullOrEmpty(sourceProduct?.OrderItemType) ? sourceProduct.OrderItemType : "Product"));
                                }
                            }
                        }
                    }
                }

                OrderInfo.DiscountSum = discountSum;
                OrderInfo.DiscountFreeItems = discountFreeItems;
                OrderInfo.FreeItems = _freeItems;
                OrderInfo.FinalSum = OrderInfo.TotalSum - OrderInfo.DiscountSum;
                OrderInfo.DiscountProcent = OrderInfo.DiscountSum * 100 / OrderInfo.TotalSum;
            }
            return checkinResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        private async Task<LoyaltyCheckinInfo> CalculateLoyaltyProgramAsync()
        {
            ResetVariablesOfLoyaltyProgram();

            var checkinResult = await CalculateCheckinAsync();

            if (!checkinResult.Ok || checkinResult.Checkin is null)
            {
                Console.WriteLine(checkinResult.HttpResponseInfo?.Message);
                return checkinResult;
            }

            var checkin = checkinResult.Checkin;

            if (!string.IsNullOrEmpty(checkin.WarningMessage))
            {
                Console.WriteLine(checkin.WarningMessage);
                return checkinResult;
            }
            else if (checkin.LoyaltyProgramResults is not null)
            {
                var products = DeliveryGeneralInfo.Products();
                double discountSum = 0;
                var discountFreeItems = new List<Guid>();
                var _freeItems = new List<Item>();

                foreach (var loyaltyProgram in checkin.LoyaltyProgramResults)
                {
                    if (loyaltyProgram.Discounts is not null)
                    {
                        foreach (var discount in loyaltyProgram.Discounts)
                        {
                            discountSum += discount.DiscountSum;
                            if (discount.Code == (int)DiscountType.FreeProduct && discount.OrderItemId is not null && discount.OrderItemId != Guid.Empty)
                                discountFreeItems.Add((Guid)discount.OrderItemId);
                        }
                    }
                    if (loyaltyProgram.FreeProducts is not null)
                    {
                        foreach (var freeProduct in loyaltyProgram.FreeProducts)
                        {
                            if (freeProduct.Products is not null)
                            {
                                foreach (var product in freeProduct.Products)
                                {
                                    var sourceProduct = products.FirstOrDefault(x => x.ItemId == product.Id);
                                    _freeItems.Add(new Item(
                                        productId: product.Id,
                                        productName: sourceProduct?.Name ?? string.Empty,
                                        amount: 1,
                                        type: !string.IsNullOrEmpty(sourceProduct?.OrderItemType) ? sourceProduct.OrderItemType : "Product"));
                                }
                            }
                        }
                    }
                }

                OrderInfo.DiscountSum = discountSum;
                OrderInfo.DiscountFreeItems = discountFreeItems;
                OrderInfo.FreeItems = _freeItems;
                OrderInfo.FinalSum = OrderInfo.TotalSum - OrderInfo.DiscountSum;
                OrderInfo.DiscountProcent = OrderInfo.DiscountSum * 100 / OrderInfo.TotalSum;
            }
            return checkinResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public async Task<bool> CalculateLoyaltyProgramAndAllowedSumAndCheckAvailableMinSumForPayAsync(ITwaNet twaNet)
        {
            var res = await CalculateLoyaltyProgramAsync(twaNet);
            if (!res.Ok || res.Checkin is null)
            {
                ResetWallet();
                return false;
            }
            else if (OrderInfo.DiscountSum > 0)
            {
                var necessarySum = 100 * DeliveryGeneralInfo.CurrOfRub / (100 - OrderInfo.DiscountProcent);
                var differenceSum = necessarySum - OrderInfo.TotalSum;
                if (differenceSum > 0)
                {
                    var popupMessage = DeliveryGeneralInfo.GetTlgWebAppPopupMessages().UnavailableMinSumWithDiscountForPay ?? throw new InfoException(typeof(OrderService).FullName!,
                        nameof(CalculateLoyaltyProgramAndAllowedSumAndCheckAvailableMinSumForPayAsync), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                        $"{nameof(TlgWebAppPopupMessages.UnavailableMinSumWithDiscountForPay)}", ExceptionType.Null);

                    popupMessage.Description = string.Format(popupMessage.Description, IntOrTwoNumberOfDigitsFromCurrentCulture(differenceSum),
                        IntOrTwoNumberOfDigitsFromCurrentCulture(necessarySum));

                    await twaNet.ShowOkPopupMessageAsync(popupMessage.Title, popupMessage.Description, HapticFeedBackNotificationType.warning);
                    return false;
                }
                if (res.Checkin?.AvailablePayments is not null)
                    CalculateAllowedWalletSum(res.Checkin.AvailablePayments);
                else
                    ResetWallet();
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public async Task<bool> CheckAvailableMinSumForPayAsync(ITwaNet twaNet)
        {
            if (OrderInfo.TotalSum < DeliveryGeneralInfo.CurrOfRub)
            {
                var popupMessage = DeliveryGeneralInfo.GetTlgWebAppPopupMessages().UnavailableMinSumtForPay ?? throw new InfoException(typeof(OrderService).FullName!,
                        nameof(CheckAvailableMinSumForPayAsync), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                        $"{nameof(TlgWebAppPopupMessages.UnavailableMinSumtForPay)}", ExceptionType.Null);

                popupMessage.Description = string.Format(popupMessage.Description, DeliveryGeneralInfo.CurrOfRub);

                await twaNet.ShowOkPopupMessageAsync(popupMessage.Title, popupMessage.Description, HapticFeedBackNotificationType.warning);
                return false;
            }
            OrderInfo.FinalSum = OrderInfo.TotalSum;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="availablePayments"></param>
        /// <returns></returns>
        private int CalculateAllowedWalletSum(IEnumerable<AvailablePayment>? availablePayments = null)
        {
            double? availableWalletSum;
            OrderInfo.AvailableWalletSum = availableWalletSum = null;

            var perhapsWalletSum = OrderInfo.FinalSum - DeliveryGeneralInfo.CurrOfRub;
            if (perhapsWalletSum <= 0)
            {
                return OrderInfo.AllowedWalletSum = 0;
            }
            else
            {
                if (availablePayments is not null)
                    availableWalletSum = CalculateAvailableWalletSum(availablePayments);

                OrderInfo.AllowedWalletSum = perhapsWalletSum > OrderInfo.WalletBalance ? (int)OrderInfo.WalletBalance : (int)perhapsWalletSum;
                if (availableWalletSum is not null)
                {
                    var value = (int)Math.Floor((double)availableWalletSum);
                    OrderInfo.AllowedWalletSum = OrderInfo.AllowedWalletSum > value ? value : OrderInfo.AllowedWalletSum;
                }
                return OrderInfo.AllowedWalletSum;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="availablePayments"></param>
        /// <returns></returns>
        private double? CalculateAvailableWalletSum(IEnumerable<AvailablePayment> availablePayments)
        {
            OrderInfo.AvailableWalletSum = null;
            double minAvailableSum = double.MaxValue;

            foreach (var availablePayment in availablePayments)
            {
                if (availablePayment.MaxSum == 0)
                {
                    minAvailableSum = 0;
                    break;
                }
                minAvailableSum = availablePayment.MaxSum <= minAvailableSum ? availablePayment.MaxSum : minAvailableSum;
            }
            if (minAvailableSum != double.MaxValue) return OrderInfo.AvailableWalletSum = minAvailableSum;
            return OrderInfo.AvailableWalletSum;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalculateAvailableWalletSum()
        {
            var perhapsWalletSum = OrderInfo.FinalSum - DeliveryGeneralInfo.CurrOfRub;

            if (perhapsWalletSum <= 0) OrderInfo.AllowedWalletSum = 0;
            else OrderInfo.AllowedWalletSum = perhapsWalletSum > OrderInfo.WalletBalance ? (int)OrderInfo.WalletBalance : (int)perhapsWalletSum;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetWallet()
        {
            OrderInfo.AvailableWalletSum = null;
            OrderInfo.AllowedWalletSum = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetVariablesOfLoyaltyProgram()
        {
            OrderInfo.DiscountSum = 0;
            OrderInfo.DiscountFreeItems.Clear();
            OrderInfo.FreeItems.Clear();
            OrderInfo.FinalSum = OrderInfo.TotalSum;
            OrderInfo.DiscountProcent = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double FinalSumOfOrder() => DeliveryGeneralInfo.UseIikoBizProgram ? OrderInfo.FinalSum : OrderInfo.TotalSum;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<WalletBalance> RetrieveWalletBalanceAsync()
        {
            var json = JsonConvert.SerializeObject(new { chatId = OrderInfo.ChatId });
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Http.PostAsync(OrderControllerPathsOfClientSide.RetreiveWalletBalance, data);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.StatusCode.Equals(HttpStatusCode.OK))
                throw new HttpProcessException(response.StatusCode, responseBody);
            return JsonConvert.DeserializeObject<WalletBalance>(responseBody) ?? throw new Exception("Json the wallet balance result is empty");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string IntOrTwoNumberOfDigitsFromCurrentCulture(double number)
            => ((int)(number * 100) % 100) != 0 ? string.Format("0:F2", number) : ((int)number).ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string IntOrTwoNumberOfDigitsFromCurrentCulture(float number)
            => ((int)(number * 100) % 100) != 0 ? string.Format("0:F2", number) : ((int)number).ToString();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public bool CheckSelectingModifiersInSelectingModifiersAndAmountsForProductPageAsync()
        {
            if (CurrentProduct is null)
                throw new InfoException(typeof(OrderService).FullName!, nameof(CheckSelectingModifiersInSelectingModifiersAndAmountsForProductPageAsync),
                    nameof(Exception), typeof(CurrentProduct).FullName!, ExceptionType.Null);
            var itemId = CurrentProduct.ProductId ?? throw new InfoException(typeof(OrderService).FullName!,
                nameof(CheckSelectingModifiersInSelectingModifiersAndAmountsForProductPageAsync), nameof(Exception),
                $"{typeof(CurrentProduct).FullName!}.{nameof(CurrentProduct.ProductId)}", ExceptionType.Null);
            var itemPositionId = CurrentProduct.ProductId ?? throw new InfoException(typeof(OrderService).FullName!,
                nameof(CheckSelectingModifiersInSelectingModifiersAndAmountsForProductPageAsync), nameof(Exception),
                $"{typeof(CurrentProduct).FullName!}.{nameof(CurrentProduct.PostionId)}", ExceptionType.Null);

            var item = OrderInfo.ItemById(itemId, itemPositionId);
            if (item is null || !item.IsReachedMinAmountOfGroupModifiers() || !item.IsReachedMinAmountOfModifiers())
                return false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public async Task<bool> IsNecessaryDataOfOrderCorrect(ITwaNet twaNet)
        {
            if (OrderInfo.FinalSum < DeliveryGeneralInfo.CurrOfRub)
            {
                var popupMessage = DeliveryGeneralInfo.GetTlgWebAppPopupMessages().UnavailableMinSumtForPay ?? throw new InfoException(typeof(OrderService).FullName!,
                    nameof(IsNecessaryDataOfOrderCorrect), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                    $"{nameof(TlgWebAppPopupMessages.UnavailableMinSumtForPay)}", ExceptionType.Null);

                popupMessage.Description = string.Format(popupMessage.Description, DeliveryGeneralInfo.CurrOfRub);
                await twaNet.ShowOkPopupMessageAsync(popupMessage.Title, popupMessage.Description, HapticFeedBackNotificationType.warning);
                return false;
            }
            else if (string.IsNullOrEmpty(OrderInfo.Address?.City) && OrderInfo.ByCourier)
            {
                var popupMessage = DeliveryGeneralInfo.GetTlgWebAppPopupMessages().IncorrectCityFormat ?? throw new InfoException(typeof(OrderService).FullName!,
                    nameof(IsNecessaryDataOfOrderCorrect), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                    $"{nameof(TlgWebAppPopupMessages.UnavailableMinSumtForPay)}", ExceptionType.Null);

                await twaNet.ShowOkPopupMessageAsync(popupMessage.Title, popupMessage.Description, HapticFeedBackNotificationType.warning);
                return false;
            }
            else if (string.IsNullOrEmpty(OrderInfo.Address?.Street) && OrderInfo.ByCourier)
            {
                var popupMessage = DeliveryGeneralInfo.GetTlgWebAppPopupMessages().IncorrectStreetFormat ?? throw new InfoException(typeof(OrderService).FullName!,
                    nameof(IsNecessaryDataOfOrderCorrect), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                    $"{nameof(TlgWebAppPopupMessages.UnavailableMinSumtForPay)}", ExceptionType.Null);

                await twaNet.ShowOkPopupMessageAsync(popupMessage.Title, popupMessage.Description, HapticFeedBackNotificationType.warning);
                return false;
            }
            else if (string.IsNullOrEmpty(OrderInfo.Address?.House) && OrderInfo.ByCourier)
            {
                var popupMessage = DeliveryGeneralInfo.GetTlgWebAppPopupMessages().IncorrectHouseFormat ?? throw new InfoException(typeof(OrderService).FullName!,
                    nameof(IsNecessaryDataOfOrderCorrect), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                    $"{nameof(TlgWebAppPopupMessages.UnavailableMinSumtForPay)}", ExceptionType.Null);

                await twaNet.ShowOkPopupMessageAsync(popupMessage.Title, popupMessage.Description, HapticFeedBackNotificationType.warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsWalletBalanceChangedInIikoCardAsync()
        {
            var walletBalance = await RetrieveWalletBalanceAsync();
            if (walletBalance?.Balance is not null && OrderInfo.WalletBalance != walletBalance.Balance)
            {
                OrderInfo.WalletBalance = walletBalance.Balance;
                CalculateAvailableWalletSum();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        public async Task<InvoiceLinkStatus> CreateInvoiceUrlLinkAsync()
        {
            try
            {
                var body = JsonConvert.SerializeObject(OrderInfo);
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await Http.PostAsync(OrderControllerPathsOfClientSide.CreateInvoiceLink, data);

                string responseBody = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new HttpProcessException(response.StatusCode, responseBody);

                return JsonConvert.DeserializeObject<InvoiceLinkStatus>(responseBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new(false, null, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public async Task<bool> CreateInvoiceLinkAsync(ITwaNet twaNet)
        {
            try
            {
                var result = await CreateInvoiceUrlLinkAsync();
                if (!result.Ok)
                {
                    await twaNet.ShowOkPopupMessageAsync(string.Empty, result.Message ?? string.Empty, HapticFeedBackNotificationType.error);
                    throw new InfoException(typeof(OrderService).FullName!, nameof(CreateInvoiceLinkAsync), nameof(Exception), result.Message ?? string.Empty);
                }
                else if (string.IsNullOrEmpty(result.InvoiceLink)) // What to send to an user abount this situation ???
                    throw new InfoException(typeof(OrderService).FullName!, nameof(CreateInvoiceLinkAsync), nameof(Exception), $"{typeof(InvoiceLinkStatus).FullName!}." +
                        $"{nameof(InvoiceLinkStatus.InvoiceLink)}", ExceptionType.NullOrEmpty);

                var invoiceClosedStatus = await twaNet.InvoiceClosedHandlerAsync(result.InvoiceLink);

                switch (invoiceClosedStatus)
                {
                    case InvoiceClosedStatus.cancelled:
                        {
                            await twaNet.SetHapticFeedbackImpactOccurredAsync(HapticFeedbackImpactOccurredType.light);
                            break;
                        }
                    case InvoiceClosedStatus.paid:
                        {
                            await twaNet.CloseWebAppAsync();
                            break;
                        }
                    default: break;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CancelCurrSelectedItemWithModifiers()
        {
            var currItem = GetCurrProduct();
            var product = DeliveryGeneralInfo.ProductById(currItem.GetProductId(), CurrentGroupId);
            var item = OrderInfo.ItemById(currItem.GetProductId(), currItem.GetPositionId());
            product.TotalAmount -= (int)item.Amount;
            OrderInfo.ZeroAmountOfItem(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task CancelCurrSimilarSelectedItemsWithModifiersAsync()
        {
            var currItem = GetCurrProduct();
            var product = DeliveryGeneralInfo.ProductById(currItem.GetProductId(), CurrentGroupId);
            product.TotalAmount = 0;
            OrderInfo.RemoveItemsById(currItem.GetProductId());
            HaveSelectedProductsAtFirst();
            await SendChangedOrderModelToServerAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public CurrentProduct GetCurrProduct() => CurrentProduct ?? throw new InfoException(typeof(OrderService).FullName!,
            nameof(GetCurrProduct), nameof(Exception), typeof(CurrentProduct).FullName!, ExceptionType.Null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SetPickupTerminalAsync(ITwaNet twaNet, Guid id)
        {
            OrderInfo.TerminalId = id;
            var pickupTerminal = DeliveryGeneralInfo.DeliveryTerminals?.FirstOrDefault(x => x.Id == id);
            if (pickupTerminal is not null)
                OrderInfo.DeliveryTerminal = new(id, pickupTerminal.Name);
            if (DeliveryGeneralInfo.UseIikoBizProgram)
                await CalculateLoayltyProgramAndAllowedSumAsync(twaNet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SetPickupTerminalAsync(Guid id)
        {
            OrderInfo.TerminalId = id;
            var pickupTerminal = DeliveryGeneralInfo.DeliveryTerminals?.FirstOrDefault(x => x.Id == id);
            if (pickupTerminal is not null)
                OrderInfo.DeliveryTerminal = new(id, pickupTerminal.Name);
            if (DeliveryGeneralInfo.UseIikoBizProgram)
                await CalculateLoayltyProgramAndAllowedSumAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task RemoveAllSelectedProductsInShoppingCartPageAsync()
        {
            OrderInfo.TotalAmount = 0;
            OrderInfo.Items?.Clear();

            if (DeliveryGeneralInfo?.TransportItemDtos is not null)
                foreach (var product in DeliveryGeneralInfo.TransportItemDtos)
                    product.TotalAmount = 0;

            await SendChangedOrderModelToServerAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string InfoAboutCreatedOrderForTest()
        {
            string selectedProductsInfo = string.Empty;
            if (OrderInfo.Items is not null)
                foreach (var item in OrderInfo.Items)
                    selectedProductsInfo += $"{item.ProductName} x{item.Amount} - ₽{item.Price}\n";

            return $"Order summary:\n" +
            $"operationId: {OrderInfo.OperationId}\n" +
            $"\n{selectedProductsInfo}\n" +
            $"Total: ₽{OrderInfo.TotalSum}\n" +
            $"Comment: {OrderInfo.Comment}\n" +
            $"Order's create date: {OrderInfo.CreatedDate}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizeId"></param>
        public Item ChangeProductSize(Guid sizeId)
        {
            var currItem = GetCurrProduct();
            var product = DeliveryGeneralInfo.ProductById(currItem.GetProductId(), CurrentGroupId);
            var item = OrderInfo.ItemById(currItem.GetProductId(), currItem.GetPositionId());
            item.ProductSizeId = sizeId;
            item.ChangePriceOfItem(product.Price(sizeId));
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        private Item GetCurrItem() => CurrItem ?? throw new InfoException(typeof(OrderService).FullName!,
            nameof(GetCurrItem), nameof(Exception), nameof(CurrItem), ExceptionType.Null);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        private TransportItemDto GetCurrProductItem() => CurrProductItem ?? throw new InfoException(typeof(OrderService).FullName!,
            nameof(GetCurrProductItem), nameof(Exception), nameof(CurrProductItem), ExceptionType.Null);
    }
}