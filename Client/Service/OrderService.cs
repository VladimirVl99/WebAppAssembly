using ApiServerForTelegram.Entities.EExceptions;
using ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TlgWebAppNet;
using WebAppAssembly.Shared.Entities;
using WebAppAssembly.Shared.Entities.CreateDelivery;
using WebAppAssembly.Shared.Entities.Exceptions;
using WebAppAssembly.Shared.Entities.IikoCloudApi;
using WebAppAssembly.Shared.Entities.OfServerSide;
using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.Entities.WebApp;
using WebAppAssembly.Shared.Models.Order;
using static System.Net.WebRequestMethods;
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

            var mainInfoTask = RetrieveMainInfoForWebAppOrderAsync(client, urlPathOfMainInfo);
            mainInfoTask.Wait();
            var mainInfo = mainInfoTask.Result;

            OrderInfo = OrderInfoInit(mainInfo.OrderInfo);
            if (OrderInfo.ChatId == 0) OrderInfo.ChatId = chatId;
            DeliveryGeneralInfo = mainInfo.DeliveryGeneralInfo ?? throw new InfoException(typeof(OrderService).FullName!,
                nameof(Exception), typeof(WebAppInfo).FullName!, ExceptionType.Null);
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
        public WebAppInfo DeliveryGeneralInfo { get; set; }
        public bool IsDiscountBalanceConfirmed { get; set; }
        public CurrentProduct? CurrentProduct { get; set; }
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
        private static async Task<MainInfoForWebAppOrder> RetrieveMainInfoForWebAppOrderAsync(HttpClient client, string urlPathOfMainInfo)
        {
            var response = await client.GetAsync(urlPathOfMainInfo);
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
                orderInfo.FreePriceItems ??= new List<Guid>();
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

            if (product.HaveModifiers() || product.HaveSizesMoreThanOne())
            {
                var positionId = AddItemToOrderWithNewPosition(product);
                var item = items.Last(x => x.ProductId == productId && x.PositionId == positionId);
                if (product.HaveSizesMoreThanOne()) item.ProductSizeId = product.ItemSizes?.FirstOrDefault()?.SizeId;
                CurrentProduct = new CurrentProduct(productId, positionId);
                AddProduct(product, item);
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
                AddProduct(product, item);
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

            RemoveProduct(product, item);

            if (!item.HaveItems())
            {
                OrderInfo.ZeroAmountOfItem(item);
                HaveSelectedItemsInOrder = OrderInfo.HaveSelectedProducts();
                await SendChangedOrderModelToServerAsync();
                return null;
            }
            await SendChangedOrderModelToServerAsync();
            return new(product, item, !HaveSelectedProductsAtFirst());
        }


        public async Task<ProductInfo> AddProductItemInShoppingCartPageAsync(ITwaNet twaNet, Guid productId, Guid? positionId = null)
        {
            var product = DeliveryGeneralInfo.ProductById(productId, CurrentGroupId);
            var item = OrderInfo.ItemById(productId, positionId);

            AddProduct(product, item);

            if (DeliveryGeneralInfo.UseIikoBizProgram)
            {
                var res = await CalculateLoyaltyProgramAsync(twaNet);
            }

            if (!IsTestMode)
            {
                await JsRuntime.InvokeVoidAsync("HapticFeedbackSelectionChangedSet");
                if (WebAppInfo.UseIikoBizProgram)
                {
                    var res = await CalculateLoyaltyProgramAsync();
                    CalculateAllowedBonusSum(OrderModel.BonusSum, res);
                }
                await JsRuntime.InvokeVoidAsync("SetPayOrderButton", WebAppInfo.UseIikoBizProgram ? OrderModel.FinalSum : OrderModel.TotalSum);
            }
            else if (WebAppInfo.UseIikoBizProgram)
            {
                var res = await CalculateLoyaltyProgramAsync();
                CalculateAllowedBonusSum(OrderModel.BonusSum, res);
            }
            await SendChangedOrderModelToServerAsync();
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
        private void AddProduct(TransportItemDto product, Item item)
        {
            OrderInfo.IncrementTotalAmountWithPrice(item);
            product.IncrementAmount(); // ??? Remove it ???
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="item"></param>
        public void RemoveProduct(TransportItemDto product, Item item)
        {
            OrderInfo.DecrementTotalAmountWithPrice(item);
            product.DecrementAmount(); // ??? Remove it ???
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task SendChangedOrderModelToServerAsync()
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
        /// Calculate discount program
        /// </summary>
        /// <returns></returns>
        private async Task<Checkin?> CalculateLoyaltyProgramAsync(ITwaNet twaNet)
        {
            var checkinResult = await CalculateCheckinAsync();

            if (!checkinResult.Ok || checkinResult.Checkin is null)
            {
                Console.WriteLine(checkinResult.HttpResponseInfo?.Message);

                var popupMessage = DeliveryGeneralInfo.GetTlgWebAppPopupMessages().LoayltyProgramUnavailable ?? throw new InfoException(typeof(OrderService).FullName!,
                    nameof(CalculateLoyaltyProgramAsync), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}.{nameof(TlgWebAppPopupMessages.LoayltyProgramUnavailable)}",
                    ExceptionType.Null);

                // !!! Need to add check of error message !!!
                if (IsReleaseMode)
                {
                    await twaNet.HideProgressAsync();
                    await twaNet.ShowOkPopupMessageAsync(popupMessage.Title ?? string.Empty, popupMessage.Description ?? string.Empty, HapticFeedBackNotificationType.warning);
                }
                return null;
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
                return null;
            }
            else if (checkin.LoyaltyProgramResults is not null)
            {
                Order.DiscountSum = 0;
                OrderModel.FreePriceItems.Clear();
                OrderModel.FreeItems.Clear();
                var products = WebAppInfo?.TransportItemDtos;

                foreach (var loyaltyProgram in checkinResult.LoyaltyProgramResults)
                {
                    if (loyaltyProgram.Discounts is not null && loyaltyProgram.Discounts.Any())
                    {
                        foreach (var discount in loyaltyProgram.Discounts)
                        {
                            OrderModel.DiscountSum += discount.DiscountSum;
                            if (discount.Code == (int)DiscountType.FreeProduct && discount.OrderItemId != null && discount.OrderItemId != Guid.Empty)
                                OrderModel.FreePriceItems.Add((Guid)discount.OrderItemId);
                        }
                    }
                    if (loyaltyProgram.FreeProducts is not null && loyaltyProgram.FreeProducts.Any())
                    {
                        foreach (var freeProduct in loyaltyProgram.FreeProducts)
                        {
                            if (freeProduct.Products is not null && freeProduct.Products.Any())
                            {
                                foreach (var product in freeProduct.Products)
                                {
                                    if (products is not null && products.Any())
                                    {
                                        var sourceProduct = products.FirstOrDefault(x => x.ItemId == product.Id);
                                        OrderModel.FreeItems.Add(new Item(
                                            productId: product.Id,
                                            productName: sourceProduct?.Name ?? string.Empty,
                                            amount: 1,
                                            type: sourceProduct?.OrderItemType ?? "Product"));
                                    }
                                }
                            }
                        }
                    }
                }

                OrderModel.FinalSum = OrderModel.TotalSum - OrderModel.DiscountSum;
                OrderModel.DiscountProcent = OrderModel.DiscountSum * 100 / OrderModel.TotalSum;
            }

            IsPageBlocked = false;
            await JsRuntime.InvokeVoidAsync("HideProgress");
            return checkinResult;

        }
    }
}