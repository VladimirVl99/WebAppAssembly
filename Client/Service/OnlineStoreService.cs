using ApiServerForTelegram.Entities.EExceptions;
using ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID;
using Microsoft.JSInterop;
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
    public class OnlineStoreService : IOnlineStoreService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="urlPathOfMainInfo"></param>
        /// <exception cref="InfoException"></exception>
        public OnlineStoreService(HttpClient client) => Http = client;

        private HttpClient Http { get; set; }
        private PersonalInfoOfOrder? _orderInfo;
        public PersonalInfoOfOrder PersonalInfoOfOrder
        {
            get => _orderInfo is null ? throw new InfoException(typeof(OnlineStoreService).FullName!, nameof(Exception),
                $"{typeof(OnlineStoreService).FullName}.{nameof(PersonalInfoOfOrder)}", ExceptionType.Null) : _orderInfo;
            set => _orderInfo = value;
        }
        private GeneralInfoOfOnlineStore? _deliveryGeneralInfo;
        public GeneralInfoOfOnlineStore GeneralInfoOfOnlineStore
        {
            get => _deliveryGeneralInfo is null ? throw new InfoException(typeof(OnlineStoreService).FullName!, nameof(Exception),
                $"{typeof(OnlineStoreService).FullName}.{nameof(PersonalInfoOfOrder)}", ExceptionType.Null) : _deliveryGeneralInfo;
            set => _deliveryGeneralInfo = value;
        }
        public CurrentProduct? CurrentProduct { get; set; }
        public Item? CurrItem { get; set; }
        public TransportItemDto? CurrProductItem { get; set; }
        public Guid? CurrentGroupId { get; set; }
        public bool IsReleaseMode { get; set; }
        private string? _tlgMainBtnColor;
        public string TlgMainBtnColor
        {
            get => _tlgMainBtnColor ?? string.Empty;
            set => _tlgMainBtnColor = value;
        }
        private bool HaveSelectedItemsInOrder { get; set; }
        public bool IsLoyaltyProgramAvailableForProcess { get; private set; }


        /// <summary>
        /// Receives and verifies data from the API server for the online store.
        /// Receives 
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="urlPathOfMainInfo"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public async Task InitOnlineStoreServiceAsync(long chatId, string urlPathOfMainInfo)
        {
            var mainInfo = await RetrieveMainInfoForWebAppOrderAsync(Http, chatId, urlPathOfMainInfo);

            GeneralInfoOfOnlineStore = mainInfo.DeliveryGeneralInfo ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                nameof(Exception), typeof(GeneralInfoOfOnlineStore).FullName!, ExceptionType.Null);
            PersonalInfoOfOrder = OrderInfoInit(mainInfo.OrderInfo);
            if (PersonalInfoOfOrder.ChatId == 0) PersonalInfoOfOrder.ChatId = chatId;
            PersonalInfoOfOrder.Address ??= new DeliveryPoint();
            IsReleaseMode = mainInfo.IsReleaseMode;
            var tlgMainBtnClr = (!string.IsNullOrEmpty(mainInfo.DeliveryGeneralInfo.TlgMainBtnColor)
                ? mainInfo.DeliveryGeneralInfo.TlgMainBtnColor : mainInfo.TlgMainBtnColor) ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                nameof(Exception), $"{typeof(MainInfoForWebAppOrder).FullName!}.{nameof(MainInfoForWebAppOrder.TlgMainBtnColor)}", ExceptionType.NullOrEmpty);
            TlgMainBtnColor = IsRgbColorFormat(tlgMainBtnClr) ? tlgMainBtnClr : throw new InfoException(typeof(OnlineStoreService).FullName!,
                nameof(Exception), $"Incorrect formant of rgb (#rrggbb) color for the main button of the Telegram. Current value is '{tlgMainBtnClr}'");
            HaveSelectedItemsInOrder = PersonalInfoOfOrder.HaveSelectedProducts();
            IsLoyaltyProgramAvailableForProcess = GeneralInfoOfOnlineStore.UseIikoBizProgram;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="urlPathOfMainInfo"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        private static async Task<MainInfoForWebAppOrder> RetrieveMainInfoForWebAppOrderAsync(HttpClient client, long chatId, string urlPathOfMainInfo)
        {
            try
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
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
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
        private static PersonalInfoOfOrder OrderInfoInit(PersonalInfoOfOrder? orderInfo = null)
        {
            if (orderInfo is not null)
            {
                orderInfo.Items ??= new List<Item>();

                if (!orderInfo.Items.Any())
                    orderInfo.WithNewParameters();

                orderInfo.FreeItems ??= new List<Item>();
                orderInfo.DiscountFreeItems ??= new List<Guid>();
                return orderInfo;
            }
            else return new PersonalInfoOfOrder();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<ProductInfo> AddOrIncreaseItemPositionForSelectingProductPageAsync(Guid productId)
        {
            var product = GeneralInfoOfOnlineStore.ProductById(productId, CurrentGroupId);
            var items = PersonalInfoOfOrder.CurrItems();

            if (product.HaveModifiersOrSeveralSizes())
            {
                var positionId = AddItemToOrderWithNewPosition(product);
                var item = items.Last(x => x.ProductId == productId && x.PositionId == positionId);
                if (product.HaveSizesMoreThanOne()) item.ProductSizeId = product.ItemSizes?.FirstOrDefault()?.SizeId;
                CurrentProduct = new CurrentProduct(productId, positionId);
                CurrItem = item;
                CurrProductItem = product;
                IncreaseAmountOfProduct(item);
                return new(item, !HaveSelectedProductsAtFirst());
            }
            else
            {
                var item = items.FirstOrDefault(x => x.ProductId == productId);
                if (item is null)
                {
                    AddItemToOrderWithNewPosition(product);
                    item = items.Last(x => x.ProductId == productId);
                }
                IncreaseAmountOfProduct(item);
                await SendChangedOrderModelToServerAsync();
                return new(item, !HaveSelectedProductsAtFirst());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<ProductInfo?> RemoveOrDecreaseItemPositionForSelectingProductPageAsync(Guid productId)
        {
            var item = PersonalInfoOfOrder.ItemByIdOrDefault(productId);
            if (item is null) return null;

            bool isRemoved;
            if (isRemoved = RemoveOrDecreaseAmountOfProduct(item))
                HaveSelectedItemsInOrder = PersonalInfoOfOrder.HaveSelectedProducts();

            await SendChangedOrderModelToServerAsync();
            return isRemoved ? null : new(item, !HaveSelectedProductsAtFirst());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<ProductInfo?> RemoveOrDecreaseItemPositionForSelectingProductPageAsync(TransportItemDto product)
        {
            var item = PersonalInfoOfOrder.ItemByIdOrDefault((Guid)product.ItemId!);
            if (item is null) return null;

            bool isRemoved;
            if (isRemoved = RemoveOrDecreaseAmountOfProduct(item))
                HaveSelectedItemsInOrder = PersonalInfoOfOrder.HaveSelectedProducts();

            await SendChangedOrderModelToServerAsync();
            return isRemoved ? null : new(item, !HaveSelectedProductsAtFirst());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public async Task<ProductInfo> IncreaseItemPositionForShoppingCartPageAsync(IJSRuntime jsRuntime, ITwaNet twaNet, Guid productId, Guid? positionId = null)
        {
            var item = PersonalInfoOfOrder.ItemById(productId, positionId);

            IncreaseAmountOfProduct(item);

            if (IsLoyaltyProgramAvailableForProcess)
                await CalculateLoayltyProgramAndAllowedSumAsync(jsRuntime, twaNet);
            else
                await SendChangedOrderModelToServerAsync();

            return new(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public async Task<ProductInfo> IncreaseItemPositionForShoppingCartPageAsync(Guid productId, Guid? positionId = null)
        {
            var item = PersonalInfoOfOrder.ItemById(productId, positionId);

            IncreaseAmountOfProduct(item);

            if (IsLoyaltyProgramAvailableForProcess)
                await CalculateLoayltyProgramAndAllowedSumAsync();
            else
                await SendChangedOrderModelToServerAsync();

            return new(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public async Task<ProductInfo?> RemoveOrDecreaseItemPositionForShoppingCartPageAsync(IJSRuntime jsRuntime, ITwaNet twaNet, Guid productId, Guid? positionId = null)
        {
            var item = PersonalInfoOfOrder.ItemById(productId, positionId);

            bool isRemoved;
            if (isRemoved = RemoveOrDecreaseAmountOfProduct(item))
                HaveSelectedItemsInOrder = PersonalInfoOfOrder.HaveSelectedProducts();

            if (IsLoyaltyProgramAvailableForProcess)
                await CalculateLoayltyProgramAndAllowedSumAsync(jsRuntime, twaNet);
            else
                await SendChangedOrderModelToServerAsync();

            if (isRemoved) return null;
            return new(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public async Task<ProductInfo?> RemoveOrDecreaseItemPositionForShoppingCartPageAsync(Guid productId, Guid? positionId = null)
        {
            var item = PersonalInfoOfOrder.ItemById(productId, positionId);

            bool isRemoved;
            if (isRemoved = RemoveOrDecreaseAmountOfProduct(item))
                HaveSelectedItemsInOrder = PersonalInfoOfOrder.HaveSelectedProducts();

            if (IsLoyaltyProgramAvailableForProcess)
                await CalculateLoayltyProgramAndAllowedSumAsync();
            else
                await SendChangedOrderModelToServerAsync();

            if (isRemoved) return null;
            return new(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveOrDecreaseItemPositionWithModifiersOrSizesForSelectingProductPageAsync(Guid productId)
        {
            if (PersonalInfoOfOrder.HaveSeveralItemPositionsOfProduct(productId))
            {
                var product = GeneralInfoOfOnlineStore.ProductById(productId, CurrentGroupId);
                CurrentProduct = new(productId);
                CurrProductItem = product;
                return true;
            }

            var item = PersonalInfoOfOrder.ItemById(productId);
            RemoveOrDecreaseAmountOfProduct(item);
            await SendChangedOrderModelToServerAsync();
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<bool> RemoveOrDecreaseItemPositionWithModifiersOrSizesForSelectingProductPageAsync(TransportItemDto product)
        {
            var item = PersonalInfoOfOrder.ItemById((Guid)product.ItemId!);

            if (PersonalInfoOfOrder.HaveSeveralItemPositionsOfProduct((Guid)product.ItemId!))
            {
                CurrentProduct = new((Guid)product.ItemId);
                CurrItem = item;
                CurrProductItem = product;
                return true;
            }

            RemoveOrDecreaseAmountOfProduct(item);
            await SendChangedOrderModelToServerAsync();
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public async Task<ProductInfo> IncreaseItemPositionWithModifiersOrSizesForChangingSelectedProductsWithModifiersPageAsync(Guid productId, Guid positionId)
        {
            var item = PersonalInfoOfOrder.ItemById(productId, positionId);
            IncreaseAmountOfProduct(item);
            await SendChangedOrderModelToServerAsync();
            return new(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public async Task<ProductInfo?> RemoveOrDecreaseItemPositionWithModifiersOrSizesForChangingSelectedProductsWithModifiersPageAsync(Guid productId, Guid positionId)
        {
            var item = PersonalInfoOfOrder.ItemById(productId, positionId);
            var isRemoved = RemoveOrDecreaseAmountOfProduct(item);
            await SendChangedOrderModelToServerAsync();
            return isRemoved ? null : new(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifierId"></param>
        /// <param name="modifierGroupId"></param>
        public Item IncreaseModifierItemPositionForSelectingModifiersAndAmountsForProductPageAsync(Guid modifierId, Guid? modifierGroupId = null)
        {
            var item = GetCurrItem();
            PersonalInfoOfOrder.IncrementTotalAmountOfModifierWithPrice(item, modifierId, modifierGroupId);
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifierId"></param>
        /// <param name="modifierGroupId"></param>
        /// <returns></returns>
        public Item DecreaseModifierItemPositionForSelectingModifiersAndAmountsForProductPageAsync(Guid modifierId, Guid? modifierGroupId = null)
        {
            var item = GetCurrItem();
            PersonalInfoOfOrder.DecrementTotalAmountOfModifierWithPrice(item, modifierId, modifierGroupId);
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public Item IncreaseItemPositionForSelectingModifiersAndAmountsForProductPageAsync()
        {
            var item = GetCurrItem();
            IncreaseAmountOfProduct(item);
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public Item? RemoveOrDecreaseItemPositionForSelectingModifiersAndAmountsForProductPageAsync()
        {
            var item = GetCurrItem();
            if (RemoveOrDecreaseAmountOfProduct(item))
                return null;
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
            var product = GeneralInfoOfOnlineStore.ProductById(productId, CurrentGroupId);
            if (product.HaveModifiersOrSeveralSizes()) throw new InfoException(typeof(OnlineStoreService).FullName!,
                nameof(AddProductWithoutModifiersForSelectingAmountsForProductsPageAsync), nameof(Exception),
                "Adding the product with modifiers for this page is not allowed");

            var item = PersonalInfoOfOrder.ItemByIdOrDefault(productId);
            if (item is null)
            {
                AddItemToOrderWithNewPosition(product);
                item = PersonalInfoOfOrder.ItemById(productId);
            }

            IncreaseAmountOfProduct(item);

            CurrentProduct = new CurrentProduct(item.ProductId);
            CurrProductItem = product;
            CurrItem = item;

            return new(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ProductInfo AddProductWithoutModifiersInSelectingAmountsForProductsPageAsync()
        {
            var item = GetCurrItem();
            IncreaseAmountOfProduct(item);
            return new(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ProductInfo? RemoveProductWithoutModifiersInSelectingAmountsForProductsPageAsync()
        {
            var item = GetCurrItem();
            if (RemoveOrDecreaseAmountOfProduct(item))
                return null;
            return new(item, !HaveSelectedProductsAtFirst());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private Guid AddItemToOrderWithNewPosition(TransportItemDto product)
        {
            var newId = Guid.NewGuid();
            PersonalInfoOfOrder.Items!.Add(new Item(product, newId));
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="item"></param>
        private void IncreaseAmountOfProduct(Item item)
        {
            PersonalInfoOfOrder.IncrementTotalAmountWithPrice(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="item"></param>
        public void DecreaseAmountOfProduct(Item item)
        {
            PersonalInfoOfOrder.DecrementTotalAmountWithPrice(item);
            HaveSelectedProductsAtFirst();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="item"></param>
        public bool RemoveOrDecreaseAmountOfProduct(Item item)
        {
            DecreaseAmountOfProduct(item);
            if (!item.HaveItems())
            {
                PersonalInfoOfOrder.ZeroAmountOfItem(item);
                HaveSelectedProductsAtFirst();
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
                var body = JsonConvert.SerializeObject(PersonalInfoOfOrder);
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
            HaveSelectedItemsInOrder = PersonalInfoOfOrder.HaveSelectedProducts();
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
            var json = JsonConvert.SerializeObject(PersonalInfoOfOrder);
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
            var walletBalance = await RetrieveWalletBalanceAsync();
            PersonalInfoOfOrder.WalletBalance = walletBalance.Balance;

            var res = await CalculateLoyaltyProgramAsync();
            if (res.Checkin?.AvailablePayments is not null)
                CalculateAllowedWalletSum(res.Checkin.AvailablePayments);
            else
                CalculateAvailableWalletSum();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        public async Task CalculateLoayltyProgramAndAllowedSumAsync(IJSRuntime jsRuntime, ITwaNet twaNet)
        {
            var res = await CalculateLoyaltyProgramAsync(jsRuntime, twaNet);
            if (res.Checkin?.AvailablePayments is not null)
                CalculateAllowedWalletSum(res.Checkin.AvailablePayments);
            else
                CalculateAvailableWalletSum();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        private async Task<LoyaltyCheckinInfo> CalculateLoyaltyProgramAsync(IJSRuntime jsRuntime, ITwaNet twaNet)
        {
            do
            {
                ResetVariablesOfLoyaltyProgram();

                var checkinResult = await CalculateCheckinAsync();

                if (!checkinResult.Ok || checkinResult.Checkin is null)
                {
                    Console.WriteLine(checkinResult.HttpResponseInfo?.Message);

                    // !!! Need to add check of error message !!!
                    if (IsReleaseMode)
                    {
                        var popupMsg = GeneralInfoOfOnlineStore.GetTlgWebAppPopupMessages().LoayltyProgramUnavailable ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                            nameof(CalculateLoyaltyProgramAsync), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}.{nameof(TlgWebAppPopupMessages.LoayltyProgramUnavailable)}",
                            ExceptionType.Null);

                        const string skip = "0";
                        const string repeat = "1";
                        const string cancel = "2";

                        var popupPrms = new PopupParams(popupMsg.Title, popupMsg.Description, new List<PopupButton>
                        {
                            new PopupButton(skip, "Пропустить", PopupButtonType.destructive),
                            new PopupButton(repeat, "Повторить", PopupButtonType.destructive),
                            new PopupButton(cancel, "Отмена", PopupButtonType.destructive)
                        });
                        var res = await twaNet!.ShowPopupParamsAsync(jsRuntime, popupPrms, HapticFeedBackNotificationType.warning, HapticFeedbackImpactOccurredType.soft);

                        switch (res)
                        {
                            case skip:
                                await twaNet.HideProgressAsync(jsRuntime);
                                return new LoyaltyCheckinInfo(ok: false)
                                {
                                    LoyaltyProgramProcessedStatus = LoyaltyProgramProcessedStatus.Skipped
                                };
                            case repeat:
                                continue;
                            default:
                                await twaNet.HideProgressAsync(jsRuntime);
                                return new LoyaltyCheckinInfo(ok: false);
                        }
                    }
                    return checkinResult;
                }

                var checkin = checkinResult.Checkin;

                if (!string.IsNullOrEmpty(checkin.WarningMessage))
                {
                    Console.WriteLine(checkin.WarningMessage);

                    if (IsReleaseMode)
                    {
                        await twaNet.HideProgressAsync(jsRuntime);
                        await twaNet.ShowOkPopupMessageAsync(jsRuntime, string.Empty, checkin.WarningMessage, HapticFeedBackNotificationType.warning);
                    }
                    return checkinResult;
                }
                else if (checkin.LoyaltyProgramResults is not null)
                {
                    var products = GeneralInfoOfOnlineStore.Products();
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

                    PersonalInfoOfOrder.DiscountSum = discountSum;
                    PersonalInfoOfOrder.DiscountFreeItems = discountFreeItems;
                    PersonalInfoOfOrder.FreeItems = _freeItems;
                    PersonalInfoOfOrder.FinalSum = PersonalInfoOfOrder.TotalSum - PersonalInfoOfOrder.DiscountSum;
                    PersonalInfoOfOrder.DiscountProcent = PersonalInfoOfOrder.DiscountSum * 100 / PersonalInfoOfOrder.TotalSum;
                }
                return checkinResult;
            } while (true);
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
                var products = GeneralInfoOfOnlineStore.Products();
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

                PersonalInfoOfOrder.DiscountSum = discountSum;
                PersonalInfoOfOrder.DiscountFreeItems = discountFreeItems;
                PersonalInfoOfOrder.FreeItems = _freeItems;
                PersonalInfoOfOrder.FinalSum = PersonalInfoOfOrder.TotalSum - PersonalInfoOfOrder.DiscountSum;
                PersonalInfoOfOrder.DiscountProcent = PersonalInfoOfOrder.DiscountSum * 100 / PersonalInfoOfOrder.TotalSum;
            }
            return checkinResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public async Task<bool> CalculateLoyaltyProgramAndAllowedSumAndCheckAvailableMinSumForPayAsync(IJSRuntime jsRuntime, ITwaNet twaNet)
        {
            IsLoyaltyProgramAvailableForProcess = true;
            PersonalInfoOfOrder.FinalSum = PersonalInfoOfOrder.TotalSum;

            if (PersonalInfoOfOrder.TotalSum < GeneralInfoOfOnlineStore.CurrOfRub)
            {
                await twaNet.HideProgressAsync(jsRuntime);
                var popupMessage = GeneralInfoOfOnlineStore.GetTlgWebAppPopupMessages().UnavailableMinSumtForPay ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                    nameof(CalculateLoyaltyProgramAndAllowedSumAndCheckAvailableMinSumForPayAsync), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                    $"{nameof(TlgWebAppPopupMessages.UnavailableMinSumtForPay)}", ExceptionType.Null);

                popupMessage.Description = string.Format(popupMessage.Description, GeneralInfoOfOnlineStore.CurrOfRub);
                await twaNet.ShowOkPopupMessageAsync(jsRuntime, popupMessage.Title, popupMessage.Description, HapticFeedBackNotificationType.warning);
                return false;
            }

            var res = await CalculateLoyaltyProgramAsync(jsRuntime, twaNet);
            if (!res.Ok || res.Checkin is null)
            {
                ResetWallet();
                if (res.LoyaltyProgramProcessedStatus == LoyaltyProgramProcessedStatus.Skipped)
                {
                    IsLoyaltyProgramAvailableForProcess = false;
                    return true;
                }
                return false;
            }
            else if (PersonalInfoOfOrder.DiscountSum > 0)
            {
                var necessarySum = 100 * GeneralInfoOfOnlineStore.CurrOfRub / (100 - PersonalInfoOfOrder.DiscountProcent);
                var differenceSum = necessarySum - PersonalInfoOfOrder.TotalSum;
                if (differenceSum > 0)
                {
                    var popupMessage = GeneralInfoOfOnlineStore.GetTlgWebAppPopupMessages().UnavailableMinSumWithDiscountForPay ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                        nameof(CalculateLoyaltyProgramAndAllowedSumAndCheckAvailableMinSumForPayAsync), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                        $"{nameof(TlgWebAppPopupMessages.UnavailableMinSumWithDiscountForPay)}", ExceptionType.Null);

                    popupMessage.Description = string.Format(popupMessage.Description, $"{differenceSum:f2}", $"{necessarySum:f2}");

                    await twaNet.HideProgressAsync(jsRuntime);
                    await twaNet.ShowOkPopupMessageAsync(jsRuntime, popupMessage.Title, popupMessage.Description, HapticFeedBackNotificationType.warning);
                    return false;
                }

                var walletBalance = await RetrieveWalletBalanceAsync();
                PersonalInfoOfOrder.WalletBalance = walletBalance.Balance;

                if (res.Checkin?.AvailablePayments is not null)
                    CalculateAllowedWalletSum(res.Checkin.AvailablePayments);
                else
                    CalculateAvailableWalletSum();
            }
            else
            {
                var walletBalance = await RetrieveWalletBalanceAsync();
                PersonalInfoOfOrder.WalletBalance = walletBalance.Balance;
                CalculateAvailableWalletSum();
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public async Task<bool> CheckAvailableMinSumForPayAsync(IJSRuntime jsRuntime, ITwaNet twaNet)
        {
            if (PersonalInfoOfOrder.TotalSum < GeneralInfoOfOnlineStore.CurrOfRub)
            {
                var popupMessage = GeneralInfoOfOnlineStore.GetTlgWebAppPopupMessages().UnavailableMinSumtForPay ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                        nameof(CheckAvailableMinSumForPayAsync), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                        $"{nameof(TlgWebAppPopupMessages.UnavailableMinSumtForPay)}", ExceptionType.Null);

                popupMessage.Description = string.Format(popupMessage.Description, GeneralInfoOfOnlineStore.CurrOfRub);

                await twaNet.ShowOkPopupMessageAsync(jsRuntime, popupMessage.Title, popupMessage.Description, HapticFeedBackNotificationType.warning);
                return false;
            }
            PersonalInfoOfOrder.FinalSum = PersonalInfoOfOrder.TotalSum;
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
            PersonalInfoOfOrder.AvailableWalletSum = availableWalletSum = null;

            var perhapsWalletSum = PersonalInfoOfOrder.FinalSum - GeneralInfoOfOnlineStore.CurrOfRub;
            if (perhapsWalletSum <= 0)
            {
                return PersonalInfoOfOrder.AllowedWalletSum = 0;
            }
            else
            {
                if (availablePayments is not null)
                    availableWalletSum = CalculateAvailableWalletSum(availablePayments);

                PersonalInfoOfOrder.AllowedWalletSum = perhapsWalletSum > PersonalInfoOfOrder.WalletBalance ? (int)PersonalInfoOfOrder.WalletBalance : (int)perhapsWalletSum;
                if (availableWalletSum is not null)
                {
                    var value = (int)Math.Floor((double)availableWalletSum);
                    PersonalInfoOfOrder.AllowedWalletSum = PersonalInfoOfOrder.AllowedWalletSum > value ? value : PersonalInfoOfOrder.AllowedWalletSum;
                }

                if (PersonalInfoOfOrder.SelectedWalletSum > PersonalInfoOfOrder.AllowedWalletSum)
                    PersonalInfoOfOrder.SelectedWalletSum = PersonalInfoOfOrder.AllowedWalletSum;

                return PersonalInfoOfOrder.AllowedWalletSum;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="availablePayments"></param>
        /// <returns></returns>
        private double? CalculateAvailableWalletSum(IEnumerable<AvailablePayment> availablePayments)
        {
            PersonalInfoOfOrder.AllowedWalletSum = 0;
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
            if (minAvailableSum != double.MaxValue) return PersonalInfoOfOrder.AllowedWalletSum = (int)minAvailableSum;
            return PersonalInfoOfOrder.AllowedWalletSum;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalculateAvailableWalletSum()
        {
            var perhapsWalletSum = PersonalInfoOfOrder.FinalSum - GeneralInfoOfOnlineStore.CurrOfRub;

            if (perhapsWalletSum <= 0) PersonalInfoOfOrder.AllowedWalletSum = 0;
            else
            {
                PersonalInfoOfOrder.AllowedWalletSum = perhapsWalletSum > PersonalInfoOfOrder.WalletBalance ? (int)PersonalInfoOfOrder.WalletBalance : (int)perhapsWalletSum;
                if (PersonalInfoOfOrder.SelectedWalletSum > PersonalInfoOfOrder.AllowedWalletSum)
                    PersonalInfoOfOrder.SelectedWalletSum = PersonalInfoOfOrder.AllowedWalletSum;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetWallet()
        {
            PersonalInfoOfOrder.AvailableWalletSum = null;
            PersonalInfoOfOrder.AllowedWalletSum = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetVariablesOfLoyaltyProgram()
        {
            PersonalInfoOfOrder.DiscountSum = 0;
            PersonalInfoOfOrder.DiscountFreeItems.Clear();
            PersonalInfoOfOrder.FreeItems.Clear();
            PersonalInfoOfOrder.FinalSum = PersonalInfoOfOrder.TotalSum;
            PersonalInfoOfOrder.DiscountProcent = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double FinalSumOfOrder()
            => IsLoyaltyProgramAvailableForProcess ? (PersonalInfoOfOrder.SelectedWalletSum > 0 ? PersonalInfoOfOrder.FinalSum - PersonalInfoOfOrder.SelectedWalletSum : PersonalInfoOfOrder.FinalSum) : PersonalInfoOfOrder.TotalSum;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<WalletBalance> RetrieveWalletBalanceAsync()
        {
            var json = JsonConvert.SerializeObject(new { chatId = PersonalInfoOfOrder.ChatId });
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Http.PostAsync(OrderControllerPathsOfClientSide.RetreiveWalletBalance, data);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.StatusCode.Equals(HttpStatusCode.OK))
                throw new HttpProcessException(response.StatusCode, responseBody);
            return JsonConvert.DeserializeObject<WalletBalance>(responseBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string IntOrTwoNumberOfDigitsFromCurrentCulture(double number)
            => ((int)(number * 100) % 100) != 0 ? string.Format("{0:F2}", number) : ((int)number).ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string IntOrTwoNumberOfDigitsFromCurrentCulture(float number)
            => ((int)(number * 100) % 100) != 0 ? string.Format("{0:F2}", number) : ((int)number).ToString();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public bool CheckSelectedModifiersInSelectingModifiersAndAmountsForProductPageAsync()
        {
            if (CurrentProduct is null)
                throw new InfoException(typeof(OnlineStoreService).FullName!, nameof(CheckSelectedModifiersInSelectingModifiersAndAmountsForProductPageAsync),
                    nameof(Exception), typeof(CurrentProduct).FullName!, ExceptionType.Null);
            var itemId = CurrentProduct.ProductId ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                nameof(CheckSelectedModifiersInSelectingModifiersAndAmountsForProductPageAsync), nameof(Exception),
                $"{typeof(CurrentProduct).FullName!}.{nameof(CurrentProduct.ProductId)}", ExceptionType.Null);
            var itemPositionId = CurrentProduct.PostionId ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                nameof(CheckSelectedModifiersInSelectingModifiersAndAmountsForProductPageAsync), nameof(Exception),
                $"{typeof(CurrentProduct).FullName!}.{nameof(CurrentProduct.PostionId)}", ExceptionType.Null);

            var item = PersonalInfoOfOrder.ItemById(itemId, itemPositionId);
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
        public async Task<bool> IsNecessaryDataOfOrderCorrect(IJSRuntime jsRuntime, ITwaNet twaNet)
        {
            if (PersonalInfoOfOrder.FinalSum < GeneralInfoOfOnlineStore.CurrOfRub)
            {
                var popupMessage = GeneralInfoOfOnlineStore.GetTlgWebAppPopupMessages().UnavailableMinSumtForPay ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                    nameof(IsNecessaryDataOfOrderCorrect), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                    $"{nameof(TlgWebAppPopupMessages.UnavailableMinSumtForPay)}", ExceptionType.Null);

                popupMessage.Description = string.Format(popupMessage.Description, GeneralInfoOfOnlineStore.CurrOfRub);
                await twaNet.ShowOkPopupMessageAsync(jsRuntime, popupMessage.Title, popupMessage.Description, HapticFeedBackNotificationType.warning);
                return false;
            }
            else if (string.IsNullOrEmpty(PersonalInfoOfOrder.Address?.City) && PersonalInfoOfOrder.ByCourier)
            {
                var popupMessage = GeneralInfoOfOnlineStore.GetTlgWebAppPopupMessages().IncorrectCityFormat ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                    nameof(IsNecessaryDataOfOrderCorrect), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                    $"{nameof(TlgWebAppPopupMessages.UnavailableMinSumtForPay)}", ExceptionType.Null);

                await twaNet.ShowOkPopupMessageAsync(jsRuntime, popupMessage.Title, popupMessage.Description, HapticFeedBackNotificationType.warning);
                return false;
            }
            else if (string.IsNullOrEmpty(PersonalInfoOfOrder.Address?.Street) && PersonalInfoOfOrder.ByCourier)
            {
                var popupMessage = GeneralInfoOfOnlineStore.GetTlgWebAppPopupMessages().IncorrectStreetFormat ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                    nameof(IsNecessaryDataOfOrderCorrect), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                    $"{nameof(TlgWebAppPopupMessages.UnavailableMinSumtForPay)}", ExceptionType.Null);

                await twaNet.ShowOkPopupMessageAsync(jsRuntime, popupMessage.Title, popupMessage.Description, HapticFeedBackNotificationType.warning);
                return false;
            }
            else if (string.IsNullOrEmpty(PersonalInfoOfOrder.Address?.House) && PersonalInfoOfOrder.ByCourier)
            {
                var popupMessage = GeneralInfoOfOnlineStore.GetTlgWebAppPopupMessages().IncorrectHouseFormat ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                    nameof(IsNecessaryDataOfOrderCorrect), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                    $"{nameof(TlgWebAppPopupMessages.UnavailableMinSumtForPay)}", ExceptionType.Null);

                await twaNet.ShowOkPopupMessageAsync(jsRuntime, popupMessage.Title, popupMessage.Description, HapticFeedBackNotificationType.warning);
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
            if (walletBalance?.Balance is not null && PersonalInfoOfOrder.WalletBalance != walletBalance.Balance)
            {
                PersonalInfoOfOrder.WalletBalance = walletBalance.Balance;
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
        public async Task<InvoiceLinkStatus> TryToCreateInvoiceUrlLinkAsync()
        {
            try
            {
                var body = JsonConvert.SerializeObject(PersonalInfoOfOrder);
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
        public async Task<bool> TryToCreateInvoiceLinkAsync(IJSRuntime jsRuntime, ITwaNet twaNet)
        {
            try
            {
                if (!IsLoyaltyProgramAvailableForProcess)
                    PersonalInfoOfOrder.FinalSum = PersonalInfoOfOrder.TotalSum;

                var result = await TryToCreateInvoiceUrlLinkAsync();
                if (!result.Ok)
                {
                    await twaNet.ShowOkPopupMessageAsync(jsRuntime, string.Empty, result.Message ?? string.Empty, HapticFeedBackNotificationType.error);
                    throw new InfoException(typeof(OnlineStoreService).FullName!, nameof(TryToCreateInvoiceLinkAsync), nameof(Exception), result.Message ?? string.Empty);
                }
                else if (string.IsNullOrEmpty(result.InvoiceLink)) // What to send to an user abount this situation ???
                    throw new InfoException(typeof(OnlineStoreService).FullName!, nameof(TryToCreateInvoiceLinkAsync), nameof(Exception), $"{typeof(InvoiceLinkStatus).FullName!}." +
                        $"{nameof(InvoiceLinkStatus.InvoiceLink)}", ExceptionType.NullOrEmpty);

                var invoiceClosedStatus = await twaNet.InvoiceClosedHandlerAsync(jsRuntime, result.InvoiceLink);

                switch (invoiceClosedStatus)
                {
                    case InvoiceClosedStatus.cancelled:
                        {
                            await twaNet.SetHapticFeedbackImpactOccurredAsync(jsRuntime, HapticFeedbackImpactOccurredType.light);
                            break;
                        }
                    case InvoiceClosedStatus.paid:
                        {
                            await twaNet.CloseWebAppAsync(jsRuntime);
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
            var item = PersonalInfoOfOrder.ItemById(currItem.GetProductId(), currItem.GetPositionId());
            PersonalInfoOfOrder.ZeroAmountOfItem(item);
            HaveSelectedProductsAtFirst();
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task CancelCurrSimilarSelectedItemsWithModifiersAsync()
        {
            var currItem = GetCurrProduct();
            PersonalInfoOfOrder.RemoveItemsById(currItem.GetProductId());
            HaveSelectedProductsAtFirst();
            await SendChangedOrderModelToServerAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public CurrentProduct GetCurrProduct() => CurrentProduct ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
            nameof(GetCurrProduct), nameof(Exception), typeof(CurrentProduct).FullName!, ExceptionType.Null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SetPickupTerminalWithLoyaltyProgramProcessAsync(IJSRuntime jsRuntime, ITwaNet twaNet, Guid id)
        {
            PersonalInfoOfOrder.TerminalId = id;
            var pickupTerminal = GeneralInfoOfOnlineStore.DeliveryTerminals?.FirstOrDefault(x => x.Id == id);
            if (pickupTerminal is not null)
                PersonalInfoOfOrder.DeliveryTerminal = new(id, pickupTerminal.Name);
            if (GeneralInfoOfOnlineStore.UseIikoBizProgram)
                await CalculateLoayltyProgramAndAllowedSumAsync(jsRuntime, twaNet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SetPickupTerminalWithLoyaltyProgramProcessAsync(Guid id)
        {
            PersonalInfoOfOrder.TerminalId = id;
            var pickupTerminal = GeneralInfoOfOnlineStore.DeliveryTerminals?.FirstOrDefault(x => x.Id == id);
            if (pickupTerminal is not null)
                PersonalInfoOfOrder.DeliveryTerminal = new(id, pickupTerminal.Name);
            if (GeneralInfoOfOnlineStore.UseIikoBizProgram)
                await CalculateLoayltyProgramAndAllowedSumAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void SetPickupTerminal(Guid id)
        {
            PersonalInfoOfOrder.TerminalId = id;
            var pickupTerminal = GeneralInfoOfOnlineStore.DeliveryTerminals?.FirstOrDefault(x => x.Id == id);
            if (pickupTerminal is not null)
                PersonalInfoOfOrder.DeliveryTerminal = new(id, pickupTerminal.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task RemoveAllSelectedProductsInShoppingCartPageAsync()
        {
            PersonalInfoOfOrder.TotalAmount = 0;
            PersonalInfoOfOrder.TotalSum = PersonalInfoOfOrder.FinalSum = 0;
            PersonalInfoOfOrder.Items?.Clear();

            HaveSelectedProductsAtFirst();
            await SendChangedOrderModelToServerAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string InfoAboutCreatedOrderForTest()
        {
            string selectedProductsInfo = string.Empty;
            if (PersonalInfoOfOrder.Items is not null)
                foreach (var item in PersonalInfoOfOrder.Items)
                    selectedProductsInfo += $"{item.ProductName} x{item.Amount} - ₽{item.Price}\n";

            return $"Order summary:\n" +
            $"operationId: {PersonalInfoOfOrder.OperationId}\n" +
            $"\n{selectedProductsInfo}\n" +
            $"Total: ₽{PersonalInfoOfOrder.TotalSum}\n" +
            $"Comment: {PersonalInfoOfOrder.Comment}\n" +
            $"Order's create date: {PersonalInfoOfOrder.CreatedDate}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizeId"></param>
        public Item ChangeProductSize(Guid sizeId)
        {
            var currItem = GetCurrProduct();
            var product = GeneralInfoOfOnlineStore.ProductById(currItem.GetProductId(), CurrentGroupId);
            var item = PersonalInfoOfOrder.ItemById(currItem.GetProductId(), currItem.GetPositionId());
            item.ProductSizeId = sizeId;
            PersonalInfoOfOrder.ChangeItemsSize(item, product.Price(sizeId));
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        private Item GetCurrItem() => CurrItem ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
            nameof(GetCurrItem), nameof(Exception), nameof(CurrItem), ExceptionType.Null);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        private TransportItemDto GetCurrProductItem() => CurrProductItem ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
            nameof(GetCurrProductItem), nameof(Exception), nameof(CurrProductItem), ExceptionType.Null);
    }
}