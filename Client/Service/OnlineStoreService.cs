using ApiServerForTelegram.Entities.EExceptions;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using TlgWebAppNet;
using WebAppAssembly.Shared.Entities.Api.Common.Delivery;
using WebAppAssembly.Shared.Entities.Api.Common.OnlineStore;
using WebAppAssembly.Shared.Entities.Api.Common.PersonalData;
using WebAppAssembly.Shared.Entities.Exceptions;
using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.Entities.WebApp;
using WebAppAssembly.Shared.Models.OrderData;
using OrderControllerPathsOfClientSide = WebAppAssembly.Shared.Entities.WebApp.OrderControllerPaths;
using TerminalInfo = WebAppAssembly.Shared.Entities.Api.Common.General.Terminals.DeliveryTerminal;
using OnlineStoreItem = WebAppAssembly.Shared.Entities.OnlineStore.OnlineStoreItem;
using WebAppAssembly.Shared.Entities.OnlineStore.Orders;
using WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.RetrieveExternalMenuByID;
using WebAppAssembly.Shared.Entities.Api.Common.IikoTransport;
using WebAppAssembly.Shared.Entities.WebAppPage;

namespace WebAppAssembly.Client.Service
{
    /// <summary>
    /// Implements the work of caluculating operations for orders in the online store on the client side.
    /// Note: There is possible to send a customer's own data of the order to the API server in the some methods.
    /// </summary>
    public class OnlineStoreService : IOnlineStoreService
    {
        #region Fields

        private readonly HttpClient _http;
        private IPersonalInfo<OrderClient>? _orderInfo;
        private OnlineStoreItem? _onlineStoreItem;
        private string? _tlgMainBtnColor;

        #endregion

        #region Properties

        public IPersonalInfo<OrderClient> PersonalInfo
        {
            get => _orderInfo is null ? throw new InfoException(typeof(OnlineStoreService).FullName!, nameof(Exception),
                $"{typeof(OnlineStoreService).FullName}.{nameof(PersonalInfo)}", ExceptionType.Null) : _orderInfo;
            private set => _orderInfo = value;
        }

        public OnlineStoreItem CommonItem
        {
            get => _onlineStoreItem is null ? throw new InfoException(typeof(OnlineStoreService).FullName!, nameof(Exception),
                $"{typeof(OnlineStoreService).FullName}.{nameof(CommonItem)}", ExceptionType.Null) : _onlineStoreItem;
            set => _onlineStoreItem = value;
        }

        public Guid? CurrentGroupId { get; private set; }

        public CurrItemPosition? CurrItemPosition { get; private set; }

        public bool IsReleaseMode { get; private set; }

        public string TlgMainBtnColor
        {
            get => _tlgMainBtnColor ?? string.Empty;
            private set => _tlgMainBtnColor = value;
        }

        public bool HaveSelectedItemsInOrder { get; private set; }

        public bool IsLoyaltyProgramAvailableForProcess { get; private set; }

        #endregion

        #region Constructors

        public OnlineStoreService(HttpClient client)
            => _http = client;

        #endregion

        /// <summary>
        /// Receives and verifies data from the API server for the online store.
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="urlPathOfMainInfo"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public async Task InitOnlineStoreServiceAsync(long chatId, string urlPathOfMainInfo)
        {
            // Retrieves the necessary data for the operation of the online store
            var common = await RetrieveMainInfoForOnlineStoreAsync(_http, chatId, urlPathOfMainInfo);

            // Extracts the necessary information for the operation of the online store
            CommonItem = new OnlineStoreItem(common.OnlineStoreItem);

            // Extracts a customer's personal data of the order
            PersonalInfo = InitPersonalInfo(common.PersonalOrderInfo);

            // Sets the opeartion mode for the online store
            IsReleaseMode = common.IsReleaseMode;

            // Extracts a color for the Tg's main button
            var tlgMainBtnClr = !string.IsNullOrWhiteSpace(common.OnlineStoreItem.TgMainBtnColor)
                ? common.OnlineStoreItem.TgMainBtnColor : throw new InfoException(typeof(OnlineStoreService).FullName!,
                nameof(Exception), $"{typeof(MainInfoForWebAppOrder).FullName!}.{nameof(MainInfoForWebAppOrder.TlgMainBtnColor)}",
                ExceptionType.NullOrEmpty);

            // Sets a color for the Tg's main button
            TlgMainBtnColor = IsRgbColorFormatCorrect(tlgMainBtnClr) ? tlgMainBtnClr : throw new InfoException(typeof(OnlineStoreService).FullName!,
                nameof(Exception), $"Incorrect formant of rgb (#rrggbb) color for the main button of the Telegram. Current value is '{tlgMainBtnClr}'");
            HaveSelectedItemsInOrder = PersonalInfo.Order.HaveSelectedItems;
            IsLoyaltyProgramAvailableForProcess = CommonItem.UseIikoBizProgram;
        }

        /// <summary>
        /// Retrieves the necessary info from the server API for the operation of the online store.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="urlPathOfMainInfo"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        private static async Task<OnlineStoreInfo> RetrieveMainInfoForOnlineStoreAsync(HttpClient client, long chatId,
            string urlPathOfMainInfo)
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

               return JsonConvert.DeserializeObject<OnlineStoreInfo>(responseBody);
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
        /// Return 'true' if an argument is rgb color format.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private static bool IsRgbColorFormatCorrect(string color)
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
        /// Initializes the personal data of the order at first render of web application.
        /// </summary>
        /// <param name="personalInfo"></param>
        /// <returns></returns>
        private static IPersonalInfo<OrderClient> InitPersonalInfo(PersonalInfo personalInfo)
            => new PersonalInfo<OrderClient>(
                order: personalInfo.Order is null ? new OrderClient() : new OrderClient(personalInfo.Order),
                chat: personalInfo.ChatInfo,
                personalItem: personalInfo.PersonalItem ?? new PersonalItem());

        public IOrderItemProcessing IncreaseModifierItemPositionForPageWhereModifiersAndSizesOfItemPositionAreSelectedAsync(
            IOrderItemProcessing item, Guid modifierId, Guid? modifierGroupId = null)
        {
            // Increases the selected modifier's number of positions of the selected product item, increases the total price of the item and order.
            PersonalInfo.Order.IncreaseTotalNumberOfModifierAndTotalPaymentAmount(item, modifierId, modifierGroupId);
            return item;
        }

        public IOrderItemProcessing DecreaseModifierItemPositionForPageWhereModifiersAndSizesOfItemPositionAreSelectedAsync(
            IOrderItemProcessing item, Guid modifierId, Guid? modifierGroupId = null)
        {
            // Decreases the selected modifier's number of positions of the selected product item, decreases the total price of the item and order.
            PersonalInfo.Order.DecreaseTotalNumberOfModifierAndTotalPaymentAmount(item, modifierId, modifierGroupId);
            return item;
        }

        /// <summary>
        /// Increases the product's number of the item position and increase the total amount for pay.
        /// </summary>
        /// <param name="item"></param>
        private void IncreaseAmountOfProduct(IOrderItemProcessing item)
        {
            PersonalInfo.Order.IncreaseTotalNumberOfItemAndTotalPaymentAmount(item);
        }

        /// <summary>
        /// Decreases the product's number of the item position and increase the total amount for pay.
        /// </summary>
        /// <param name="item"></param>
        private void DecreaseAmountOfProduct(IOrderItemProcessing item)
        {
            PersonalInfo.Order.DecreaseTotalNumberOfItemAndTotalPaymentAmount(item);
            HaveSelectedProductsAtFirst();
        }

        /// <summary>
        /// Decreases the product's number of the item position and increase the total amount for pay.
        /// Removes the item if the number of positions is equal to zero.
        /// Return 'true' if the item is removed.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="item"></param>
        private bool RemoveOrDecreaseAmountOfProduct(IOrderItemProcessing item)
        {
            DecreaseAmountOfProduct(item);
            if (!item.HaveItems)
            {
                PersonalInfo.Order.ZeroAmountOfItem(item);
                HaveSelectedProductsAtFirst();
                return true;
            }
            return false;
        }

        public async Task SavePersonalDataOfOrderInServerAsync()
        {
            try
            {
                var body = JsonConvert.SerializeObject(PersonalInfo);
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync(OrderControllerPathsOfClientSide.SendChangedOrderModelToServer, data);

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
        /// Returns 'true' when the first product item in the order was added.
        /// </summary>
        /// <returns></returns>
        public bool HaveSelectedProductsAtFirst()
        {
            bool val = HaveSelectedItemsInOrder;
            HaveSelectedItemsInOrder = PersonalInfo.Order.HaveSelectedItems;
            return val;
        }

        /// <summary>
        /// Caluculates the checkin for the order (loaylty program).
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="HttpProcessException"></exception>
        private async Task<LoyaltyCheckinInfo> CalculateCheckinAsync()
        {
            var json = JsonConvert.SerializeObject(PersonalInfo);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(OrderControllerPathsOfClientSide.CalculateCheckin, data);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.StatusCode.Equals(HttpStatusCode.OK))
                throw new HttpProcessException(response.StatusCode, responseBody);
            return JsonConvert.DeserializeObject<LoyaltyCheckinInfo>(responseBody);
        }

        public async Task CalculateCheckinAndAllowedSumAsync((IJSRuntime, ITwaNet)? jsProcessing = null)
        {
            var res = jsProcessing is not null ? await CalculateLoyaltyProgramAsync(jsProcessing.Value.Item1, jsProcessing.Value.Item2)
                : await CalculateLoyaltyProgramAsync();
            if (res.Checkin?.AvailablePayments is not null)
                CalculateAllowedWalletSum(res.Checkin.AvailablePayments);
            else
                CalculateAvailableWalletSum();
        }

        /// <summary>
        /// Caluculates the checkin for the order (loaylty program).
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        private async Task<LoyaltyCheckinInfo> CalculateLoyaltyProgramAsync(IJSRuntime jsRuntime, ITwaNet twaNet)
        {
            do
            {
                // Resets all variables of the loaylty program except the customer's wallet balance
                PersonalInfo.Order.ResetDiscountItemsWithDiscountAmounts();

                // Calculates the loaylty program in the API server
                var checkinResult = await CalculateCheckinAsync();

                // Receives some error while processing the checkin
                if (!checkinResult.Ok || checkinResult.Checkin is null)
                {
                    Console.WriteLine(checkinResult.HttpResponseInfo?.Message);

                    // !!! Need to add check of error message !!!
                    if (IsReleaseMode)
                    {
                        // Popup message of an unavailable of checkin processing
                        var popupMsg = CommonItem.TgWebAppPopupMessages?.LoayltyProgramUnavailable
                            ?? throw new InfoException(typeof(OnlineStoreService).FullName!, nameof(CalculateLoyaltyProgramAsync),
                            nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}.{nameof(TlgWebAppPopupMessages.LoayltyProgramUnavailable)}",
                            ExceptionType.Null);

                        const string skip = "0";
                        const string repeat = "1";
                        const string cancel = "2";

                        // Option of a choice
                        var popupPrms = new PopupParams(popupMsg.Title, popupMsg.Description, new List<PopupButton>
                        {
                            new PopupButton(cancel, "Отмена", PopupButtonType.destructive),
                            new PopupButton(skip, "Пропустить", PopupButtonType.destructive),
                            new PopupButton(repeat, "Повторить", PopupButtonType.destructive)
                        });
                        // Receives the customer's answer
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

                // Have some warnings in the checkin result
                if (!string.IsNullOrEmpty(checkin.WarningMessage))
                {
                    Console.WriteLine(checkin.WarningMessage);

                    if (IsReleaseMode)
                    {
                        await twaNet.HideProgressAsync(jsRuntime);
                        // Shows the warning in the popup message
                        await twaNet.ShowOkPopupMessageAsync(jsRuntime, string.Empty, checkin.WarningMessage, HapticFeedBackNotificationType.warning);
                    }
                    return checkinResult;
                }
                else if (checkin.LoyaltyProgramResults is not null)
                {
                    var products = CommonItem.Menus;
                    // Discount amount of the order
                    double discountSum = 0;
                    // Free items in the selected positions
                    var discountFreeItems = new List<Guid>();
                    // Free items
                    var freeItems = new List<IOrderItemProcessing>();

                    foreach (var loyaltyProgram in checkin.LoyaltyProgramResults)
                    {
                        if (loyaltyProgram.Discounts is not null)
                        {
                            // Calculates the discount amount and adds free item IDs
                            foreach (var discount in loyaltyProgram.Discounts)
                            {
                                discountSum += discount.DiscountSum;
                                if (discount.Code == (int)DiscountType.FreeProduct && discount.OrderItemId is not null && discount.OrderItemId != Guid.Empty)
                                    discountFreeItems.Add((Guid)discount.OrderItemId);
                            }
                        }
                        if (loyaltyProgram.FreeProducts is not null)
                        {
                            // Adds free items to the order
                            foreach (var freeProduct in loyaltyProgram.FreeProducts)
                            {
                                if (freeProduct.Products is not null)
                                {
                                    foreach (var product in freeProduct.Products)
                                    {
                                        var sourceProduct = products?.FirstOrDefault(x => x.ItemId == product.Id);

                                        if (sourceProduct is not null)
                                        {
                                            freeItems.Add(new WebAppAssembly.Shared.Entities.OnlineStore.Orders.OrderItem(
                                                product.Id,
                                                sourceProduct.Name,
                                                amount: 1,
                                                sourceProduct.OrderItemType));
                                        }
                                    }
                                }
                            }
                        }
                    }

                    PersonalInfo.Order.SetDiscountAmount(discountSum);
                    PersonalInfo.Order.SetDiscountFreeItems(discountFreeItems);
                    PersonalInfo.Order.SetFreeItems(freeItems);
                }
                return checkinResult;
            }
            while (true);
        }

        /// <summary>
        /// Caluculates the checkin for the order (loaylty program).
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        private async Task<LoyaltyCheckinInfo> CalculateLoyaltyProgramAsync()
        {
            // Resets all variables of the loaylty program except the customer's wallet balance
            PersonalInfo.Order.ResetDiscountItemsWithDiscountAmounts();

            // Calculates the loaylty program in the API server
            var checkinResult = await CalculateCheckinAsync();

            // Receives some error while processing the checkin
            if (!checkinResult.Ok || checkinResult.Checkin is null)
            {
                Console.WriteLine(checkinResult.HttpResponseInfo?.Message);
                return checkinResult;
            }

            var checkin = checkinResult.Checkin;

            // Have some warnings in the checkin result
            if (!string.IsNullOrEmpty(checkin.WarningMessage))
            {
                Console.WriteLine(checkin.WarningMessage);
                return checkinResult;
            }
            else if (checkin.LoyaltyProgramResults is not null)
            {
                var products = CommonItem.Menus;
                // Discount amount of the order
                double discountSum = 0;
                // Free items in the selected positions
                var discountFreeItems = new List<Guid>();
                // Free items
                var _freeItems = new List<IOrderItemProcessing>();

                foreach (var loyaltyProgram in checkin.LoyaltyProgramResults)
                {
                    // Calculates the discount amount and adds free item IDs
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
                        // Adds free items to the order
                        foreach (var freeProduct in loyaltyProgram.FreeProducts)
                        {
                            if (freeProduct.Products is not null)
                            {
                                foreach (var product in freeProduct.Products)
                                {
                                    var sourceProduct = products?.FirstOrDefault(x => x.ItemId == product.Id);

                                    if (sourceProduct is not null)
                                    {
                                        _freeItems.Add(new WebAppAssembly.Shared.Entities.OnlineStore.Orders.OrderItem(
                                            product.Id,
                                            sourceProduct.Name,
                                            amount: 1,
                                            sourceProduct.OrderItemType));
                                    }
                                }
                            }
                        }
                    }
                }

                PersonalInfo.Order.SetDiscountAmount(discountSum);
                PersonalInfo.Order.SetDiscountFreeItems(discountFreeItems);
                PersonalInfo.Order.SetFreeItems(_freeItems);
            }
            return checkinResult;
        }

        public async Task<bool> CalculateCheckinAndAllowedSumAndCheckAvailableMinSumForPayAsync(IJSRuntime jsRuntime, ITwaNet twaNet)
        {
            IsLoyaltyProgramAvailableForProcess = true;
            PersonalInfo.Order.ResetFinalPaymentAmount();

            // Order amount cannot be less than the min amount to pay in the Telegram
            if (PersonalInfo.Order.PaymentAmountOfSeletedItems < CommonItem.MinPaymentAmountInRubOfTg)
            {
                await twaNet.HideProgressAsync(jsRuntime);
                // Popup message of impossible to pay the order becouse the order amount is less than needs
                var popupMessage = CommonItem.TgWebAppPopupMessages?.UnavailableMinSumtForPay ?? throw new InfoException(
                    typeof(OnlineStoreService).FullName!, nameof(CalculateCheckinAndAllowedSumAndCheckAvailableMinSumForPayAsync),
                    nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                    $"{nameof(TlgWebAppPopupMessages.UnavailableMinSumtForPay)}", ExceptionType.Null);

                popupMessage.Description = string.Format(popupMessage.Description, CommonItem.MinPaymentAmountInRubOfTg);
                await twaNet.ShowOkPopupMessageAsync(jsRuntime, popupMessage.Title, popupMessage.Description,
                    HapticFeedBackNotificationType.warning);
                return false;
            }

            // 
            var res = await CalculateLoyaltyProgramAsync(jsRuntime, twaNet);
            if (!res.Ok || res.Checkin is null)
            {
                PersonalInfo.Order.ResetWalletAmounts();

                // Continues to work with the order without the loyalty program
                if (res.LoyaltyProgramProcessedStatus == LoyaltyProgramProcessedStatus.Skipped)
                {
                    IsLoyaltyProgramAvailableForProcess = false;
                    return true;
                }
                return false;
            }
            else if (PersonalInfo.Order.DiscountAmount > 0)
            {
                // Calculates the min order amount with the discount
                var necessarySum = 100 * CommonItem.MinPaymentAmountInRubOfTg / (100 - PersonalInfo.Order.DiscountProcent);
                var differenceSum = necessarySum - PersonalInfo.Order.PaymentAmountOfSeletedItems;
                if (differenceSum > 0)
                {
                    // Popup message of impossible to pay the order becouse the order amount with discounts is less than needs
                    var popupMessage = CommonItem.TgWebAppPopupMessages?.UnavailableMinSumWithDiscountForPay
                        ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                        nameof(CalculateCheckinAndAllowedSumAndCheckAvailableMinSumForPayAsync), nameof(Exception),
                        $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                        $"{nameof(TlgWebAppPopupMessages.UnavailableMinSumWithDiscountForPay)}", ExceptionType.Null);

                    popupMessage.Description = string.Format(popupMessage.Description, $"{differenceSum:f2}", $"{necessarySum:f2}");

                    await twaNet.HideProgressAsync(jsRuntime);
                    await twaNet.ShowOkPopupMessageAsync(jsRuntime, popupMessage.Title, popupMessage.Description, HapticFeedBackNotificationType.warning);
                    return false;
                }

                var walletBalance = await RetrieveWalletBalanceAsync();
                PersonalInfo.Order.SetWalletBalance(walletBalance.Balance);

                if (res.Checkin?.AvailablePayments is not null)
                    CalculateAllowedWalletSum(res.Checkin.AvailablePayments);
                else
                    CalculateAvailableWalletSum();
            }
            else
            {
                var walletBalance = await RetrieveWalletBalanceAsync();
                PersonalInfo.Order.SetWalletBalance(walletBalance.Balance);
                CalculateAvailableWalletSum();
            }

            return true;
        }

        public async Task<bool> IsOrderTotalSumMoreOrEqualMinSumForPayAsync(IJSRuntime jsRuntime, ITwaNet twaNet)
        {
            if (PersonalInfo.Order.PaymentAmountOfSeletedItems < CommonItem.MinPaymentAmountInRubOfTg)
            {
                // Popup message of impossible to pay the order becouse the order amount is less than needs
                var popupMessage = CommonItem.TgWebAppPopupMessages?.UnavailableMinSumtForPay ?? throw new InfoException(
                    typeof(OnlineStoreService).FullName!, nameof(IsOrderTotalSumMoreOrEqualMinSumForPayAsync), nameof(Exception),
                    $"{typeof(TlgWebAppPopupMessages).FullName!}.{nameof(TlgWebAppPopupMessages.UnavailableMinSumtForPay)}",
                    ExceptionType.Null);

                popupMessage.Description = string.Format(popupMessage.Description, CommonItem.MinPaymentAmountInRubOfTg);

                await twaNet.ShowOkPopupMessageAsync(jsRuntime, popupMessage.Title, popupMessage.Description, HapticFeedBackNotificationType.warning);
                return false;
            }
            PersonalInfo.Order.ResetFinalPaymentAmount();
            return true;
        }

        /// <summary>
        /// Calculates the allowed bonus sum to pay in the order.
        /// </summary>
        /// <param name="availablePayments"></param>
        /// <returns></returns>
        private int CalculateAllowedWalletSum(IEnumerable<AvailablePayment>? availablePayments = null)
        {
            double? availableWalletAmount = null;
            PersonalInfo.Order.SetAvailableWalletAmount(availableWalletAmount);

            // The amount of bonuses that can be applied
            var perhapsWalletAmount = PersonalInfo.Order.TotalPaymentAmount - CommonItem.MinPaymentAmountInRubOfTg;
            // Bonuses cannot be used
            if (perhapsWalletAmount <= 0)
            {
                PersonalInfo.Order.ResetAllowedWalletAmount();
                return 0;
            }
            else
            {
                // Finds a necessary min amount of bonuses that can be applied
                if (availablePayments is not null)
                    availableWalletAmount = CalculateAvailableWalletSum(availablePayments);

                PersonalInfo.Order.SetAllowedWalletAmount(perhapsWalletAmount > PersonalInfo.Order.WalletBalance ? (int)PersonalInfo.Order.WalletBalance
                    : (int)perhapsWalletAmount);

                if (availableWalletAmount is not null)
                {
                    var flooredAvailableWalletAmount = (int)Math.Floor((double)availableWalletAmount);
                    PersonalInfo.Order.SetAllowedWalletAmount(PersonalInfo.Order.AllowedWalletAmount > flooredAvailableWalletAmount ? flooredAvailableWalletAmount : PersonalInfo.Order.AllowedWalletAmount);
                }

                if (PersonalInfo.Order.SelectedNumberOfBonuses > PersonalInfo.Order.AllowedWalletAmount)
                    PersonalInfo.Order.SetSelectedWalletAmount(PersonalInfo.Order.AllowedWalletAmount);

                return PersonalInfo.Order.AllowedWalletAmount;
            }
        }

        /// <summary>
        /// Finds a min available amount with discount for the bonus.
        /// </summary>
        /// <param name="availablePayments"></param>
        /// <returns></returns>
        private int CalculateAvailableWalletSum(IEnumerable<AvailablePayment> availablePayments)
        {
            PersonalInfo.Order.ResetAllowedWalletAmount();
            double minAvailableAmount = double.MaxValue;

            foreach (var availablePayment in availablePayments)
            {
                if (availablePayment.MaxSum == 0)
                {
                    minAvailableAmount = 0;
                    break;
                }
                minAvailableAmount = availablePayment.MaxSum <= minAvailableAmount ? availablePayment.MaxSum : minAvailableAmount;
            }
            if (minAvailableAmount != double.MaxValue)
            {
                var intMinAvailableAmount = (int)minAvailableAmount;
                PersonalInfo.Order.SetAllowedWalletAmount(intMinAvailableAmount);
                return intMinAvailableAmount;
            }
            return PersonalInfo.Order.AllowedWalletAmount;
        }

        /// <summary>
        /// Caclulates a min available amount for the bonus.
        /// </summary>
        private void CalculateAvailableWalletSum()
        {
            var perhapsWalletSum = PersonalInfo.Order.TotalPaymentAmount - CommonItem.MinPaymentAmountInRubOfTg;

            if (perhapsWalletSum <= 0)
            {
                PersonalInfo.Order.ResetAllowedWalletAmount();
            }
            else
            {
                PersonalInfo.Order.SetAllowedWalletAmount(perhapsWalletSum > PersonalInfo.Order.WalletBalance
                    ? (int)PersonalInfo.Order.WalletBalance : (int)perhapsWalletSum);
                if (PersonalInfo.Order.SelectedNumberOfBonuses > PersonalInfo.Order.AllowedWalletAmount)
                    PersonalInfo.Order.SetSelectedWalletAmount(PersonalInfo.Order.AllowedWalletAmount);
            }
        }

        public double FinalPaymentAmount()
            => IsLoyaltyProgramAvailableForProcess
            ? PersonalInfo.Order.FinalPaymentAmountWithSelectedWalletAmount()
            : PersonalInfo.Order.PaymentAmountOfSeletedItems;

        public async Task<WalletBalance> RetrieveWalletBalanceAsync()
        {
            var json = JsonConvert.SerializeObject(new { chatId = PersonalInfo.ChatInfo.ChatId });
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(OrderControllerPathsOfClientSide.RetreiveWalletBalance, data);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.StatusCode.Equals(HttpStatusCode.OK))
                throw new HttpProcessException(response.StatusCode, responseBody);
            return JsonConvert.DeserializeObject<WalletBalance>(responseBody);
        }

        /// <summary>
        /// !!! Unnecessary method !!!
        /// Needs to check required modifiers of an item in processing it.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public bool IsSelectedModifiersOfCurrentItemPositionCorrect()
        {
            var item = CurrItemPosition?.Item;
            if (item is null)
                throw new InfoException(typeof(OnlineStoreService).FullName!, nameof(IsSelectedModifiersOfCurrentItemPositionCorrect),
                    nameof(Exception), "The current item can't be null");

            if (item.PositionId is null)
                throw new InfoException(typeof(OnlineStoreService).FullName!, nameof(IsSelectedModifiersOfCurrentItemPositionCorrect),
                    nameof(Exception), $"Position ID of the item by productId - {item.ProductId} can't be null");

            if (!item.IsMinAmountOfGroupModifiersReached() || !item.IsMinAmountOfModifiersReached())
                return false;
            return true;
        }

        public async Task<bool> IsNecessaryDataOfOrderCorrectAsync(IJSRuntime jsRuntime, ITwaNet twaNet)
        {
            // The total amount of the order to be paid is less than the minimum payment rate
            if (PersonalInfo.Order.TotalPaymentAmount < CommonItem.MinPaymentAmountInRubOfTg)
            {
                var popupMessage = CommonItem.TgWebAppPopupMessages?.UnavailableMinSumtForPay
                    ?? throw new InfoException(typeof(OnlineStoreService).FullName!, nameof(IsNecessaryDataOfOrderCorrectAsync), nameof(Exception),
                    $"{typeof(TlgWebAppPopupMessages).FullName!}.{nameof(TlgWebAppPopupMessages.UnavailableMinSumtForPay)}", ExceptionType.Null);

                popupMessage.Description = string.Format(popupMessage.Description, CommonItem.MinPaymentAmountInRubOfTg);
                await twaNet.ShowOkPopupMessageAsync(jsRuntime, popupMessage.Title, popupMessage.Description,
                    HapticFeedBackNotificationType.warning);

                return false;
            }
            else if (string.IsNullOrEmpty(PersonalInfo.PersonalItem.Address?.City)
                && PersonalInfo.PersonalItem.DeliveryServiceType == DeliveryServiceType.DeliveryByCourier)
            {
                var popupMessage = CommonItem.TgWebAppPopupMessages?.IncorrectCityFormat
                    ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                    nameof(IsNecessaryDataOfOrderCorrectAsync), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                    $"{nameof(TlgWebAppPopupMessages.IncorrectCityFormat)}", ExceptionType.Null);

                await twaNet.ShowOkPopupMessageAsync(jsRuntime, popupMessage.Title, popupMessage.Description,
                    HapticFeedBackNotificationType.warning);

                return false;
            }
            else if (string.IsNullOrEmpty(PersonalInfo.PersonalItem.Address?.Street)
                && PersonalInfo.PersonalItem.DeliveryServiceType == DeliveryServiceType.DeliveryByCourier)
            {
                var popupMessage = CommonItem.TgWebAppPopupMessages?.IncorrectStreetFormat
                    ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                    nameof(IsNecessaryDataOfOrderCorrectAsync), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                    $"{nameof(TlgWebAppPopupMessages.IncorrectStreetFormat)}", ExceptionType.Null);

                await twaNet.ShowOkPopupMessageAsync(jsRuntime, popupMessage.Title, popupMessage.Description,
                    HapticFeedBackNotificationType.warning);

                return false;
            }
            else if (string.IsNullOrEmpty(PersonalInfo.PersonalItem.Address?.House)
                && PersonalInfo.PersonalItem.DeliveryServiceType == DeliveryServiceType.DeliveryByCourier)
            {
                var popupMessage = CommonItem.TgWebAppPopupMessages?.IncorrectHouseFormat
                    ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                    nameof(IsNecessaryDataOfOrderCorrectAsync), nameof(Exception), $"{typeof(TlgWebAppPopupMessages).FullName!}." +
                    $"{nameof(TlgWebAppPopupMessages.IncorrectHouseFormat)}", ExceptionType.Null);

                await twaNet.ShowOkPopupMessageAsync(jsRuntime, popupMessage.Title, popupMessage.Description,
                    HapticFeedBackNotificationType.warning);

                return false;
            }
            return true;
        }

        public async Task<bool> IsWalletBalanceChangedAfterLastRequestAsync()
        {
            var walletBalance = await RetrieveWalletBalanceAsync();
            if (walletBalance?.Balance is not null && PersonalInfo.Order.WalletBalance != walletBalance.Balance)
            {
                PersonalInfo.Order.SetWalletBalance(walletBalance.Balance);
                CalculateAvailableWalletSum();
                return true;
            }
            return false;
        }

        public async Task<InvoiceLinkStatus> TryToCreateInvoiceUrlLinkAsync()
        {
            try
            {
                var body = JsonConvert.SerializeObject(PersonalInfo);
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync(OrderControllerPathsOfClientSide.CreateInvoiceLink, data);

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

        public async Task<bool> TryToCreateInvoiceLinkAsync(IJSRuntime jsRuntime, ITwaNet twaNet)
        {
            try
            {
                if (!IsLoyaltyProgramAvailableForProcess)
                    PersonalInfo.Order.ResetFinalPaymentAmount();

                var result = await TryToCreateInvoiceUrlLinkAsync();
                if (!result.Ok)
                {
                    await twaNet.ShowOkPopupMessageAsync(jsRuntime, string.Empty, result.Message ?? string.Empty, HapticFeedBackNotificationType.error);
                    throw new InfoException(typeof(OnlineStoreService).FullName!, nameof(TryToCreateInvoiceLinkAsync), nameof(Exception), result.Message ?? string.Empty);
                }
                else if (string.IsNullOrEmpty(result.InvoiceLink)) // What to send to an user abount this situation ???
                    throw new InfoException(typeof(OnlineStoreService).FullName!, nameof(TryToCreateInvoiceLinkAsync), nameof(Exception), $"{typeof(InvoiceLinkStatus).FullName!}." +
                        $"{nameof(InvoiceLinkStatus.InvoiceLink)}", ExceptionType.NullOrEmpty);

                // Gets a status result of the payment processing 
                var invoiceClosedStatus = await twaNet.InvoiceClosedHandlerAsync(jsRuntime, result.InvoiceLink);

                switch (invoiceClosedStatus)
                {
                    // Goes back to the order for editing
                    case InvoiceClosedStatus.cancelled:
                        {
                            await twaNet.HapticFeedbackImpactOccurredAsync(jsRuntime, HapticFeedbackImpactOccurredType.light);
                            break;
                        }
                    // The order is paid. Closes the web application
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

        public void CancelCurrSelectedItemWithModifiers()
        {
            var item = CurrItemPosition?.Item ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                nameof(CancelCurrSelectedItemWithModifiers), nameof(Exception), "The current item cannot be null");

            PersonalInfo.Order.ZeroAmountOfItem(item);
            HaveSelectedProductsAtFirst();
        }

        public async Task CancelCurrSimilarSelectedItemsWithModifiersAsync()
        {
            var product = CurrItemPosition?.ProductInfo ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                nameof(CancelCurrSimilarSelectedItemsWithModifiersAsync), nameof(Exception), "The current item cannot be null");

            PersonalInfo.Order.RemoveItemsById(product.ItemId);
            HaveSelectedProductsAtFirst();
            await SavePersonalDataOfOrderInServerAsync();
        }

        public async Task SetPickupTerminalWithCalculatingCheckinAsync(IJSRuntime jsRuntime, ITwaNet twaNet, Guid id)
        {
            var pickupTerminal = CommonItem.DeliveryTerminals?.FirstOrDefault(x => x.Id == id)
                ?? throw new Exception($"No found terminal by ID - {id}");

            PersonalInfo.PersonalItem.DeliveryTerminal = new TerminalInfo()
            {
                Id = pickupTerminal.Id,
                Name = pickupTerminal.Name ?? string.Empty,
            };

            if (IsLoyaltyProgramAvailableForProcess)
                await CalculateCheckinAndAllowedSumAsync((jsRuntime, twaNet));
        }

        public async Task SetPickupTerminalWithLoyaltyProgramProcessAsync(Guid id)
        {
            var pickupTerminal = CommonItem.DeliveryTerminals?.FirstOrDefault(x => x.Id == id)
                ?? throw new Exception($"No found terminal by ID - {id}");

            PersonalInfo.PersonalItem.DeliveryTerminal = new TerminalInfo()
            {
                Id = pickupTerminal.Id,
                Name = pickupTerminal.Name ?? string.Empty,
            };

            if (IsLoyaltyProgramAvailableForProcess)
                await CalculateCheckinAndAllowedSumAsync();
        }

        public void SetPickupTerminal(Guid id)
        {
            var pickupTerminal = CommonItem.DeliveryTerminals?.FirstOrDefault(x => x.Id == id)
                ?? throw new Exception($"No found terminal by ID - {id}");

            PersonalInfo.PersonalItem.DeliveryTerminal = new TerminalInfo()
            {
                Id = pickupTerminal.Id,
                Name = pickupTerminal.Name ?? string.Empty,
            };
        }

        public async Task RemoveAllSelectedProductsFromBasketAsync()
        {
            PersonalInfo.Order.ClearBasketOfOrder();
            HaveSelectedItemsInOrder = false;
            // Save the info about the order in the API server
            await SavePersonalDataOfOrderInServerAsync();
        }

        public string InfoAboutCreatedOrderForTest()
        {
            string selectedProductsInfo = string.Empty;
            if (PersonalInfo.Order.Items is not null)
                foreach (var item in PersonalInfo.Order.Items)
                    selectedProductsInfo += $"{item.ProductName} x{item.Amount} - ₽{item.Price}\n";

            return $"Order summary:\n" +
            $"operationId: {PersonalInfo.Order.OperationId}\n" +
            $"\n{selectedProductsInfo}\n" +
            $"Total: ₽{PersonalInfo.Order.PaymentAmountOfSeletedItems}\n" +
            $"Comment: {PersonalInfo.Order.Comment}\n" +
            $"Order's create date: {PersonalInfo.Order.CreatedDate}";
        }

        public IOrderItemProcessing ChangeProductSize(Guid sizeId)
        {
            if (CurrItemPosition?.Item is null)
                throw new InfoException(typeof(OnlineStoreService).FullName!, nameof(ChangeProductSize), nameof(Exception),
                    "The current item can't be null");

            PersonalInfo.Order.ChangeSizeOfItem(CurrItemPosition.Item, CurrItemPosition.ProductInfo.Price(sizeId), sizeId);
            return CurrItemPosition.Item;
        }

        /// <summary>
        /// Adds a new item for a product that contains modifiers and sizes every time this method is called.
        /// Adds a new item for a product without modifiers and sizes then increases the quantity for this item.
        /// Also returns the bool value that the first item has been added.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private async Task<(IOrderItemProcessing Item, bool IsFirstItemHasBeenAdded)> AddOrIncreaseItemPositionForPageWhereProductsAreSelectedAsync(
            Guid productId)
        {
            // Gets the product by ID and the group ID
            var product = CommonItem.ProductById(productId, CurrentGroupId);
            // Adds a new position or increases the product's number of position
            return await AddOrIncreaseItemPositionForPageWhereProductsAreSelectedAsync(product);
        }

        /// <summary>
        /// Adds a new item for a product that contains modifiers and sizes every time this method is called.
        /// Adds a new item for a product without modifiers and sizes then increases the quantity for this item.
        /// Also returns the bool value that the first item has been added.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private async Task<(IOrderItemProcessing Item, bool IsFirstItemHasBeenAdded)> AddOrIncreaseItemPositionForPageWhereProductsAreSelectedAsync(
            TransportItemDto product)
        {
            // The list of the added positions
            var items = PersonalInfo.Order.Items;

            if (product.HaveModifiersOrSeveralSizes())
            {
                // Adds a new item position
                var item = PersonalInfo.Order.AddItemWithNewPosition(product, product.ItemSizes?.FirstOrDefault()?.SizeId);
                // Sets the current item position to the order
                CurrItemPosition = new(product, item);
                IncreaseAmountOfProduct(item);
                return new(item, !HaveSelectedProductsAtFirst());
            }
            else
            {
                var item = items.FirstOrDefault(x => x.ProductId == product.ItemId) ?? PersonalInfo.Order.AddItemWithNewPosition(product);
                IncreaseAmountOfProduct(item);
                // Save the action data of this web app in a API server
                await SavePersonalDataOfOrderInServerAsync();
                return (item, !HaveSelectedProductsAtFirst());
            }
        }

        /// <summary>
        /// Increases the number of positions of the selected product item.
        /// Recalculates the checkin with the allowed bonus sum to pay when the loyalty system is used.
        /// Supports operation only when working with the Telegram application, in others cases it is not recommended for use.
        /// </summary>
        /// <param name="jsProcessing"></param>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        private async Task<IOrderItemProcessing> IncreaseItemPositionForBasketPageAsync((IJSRuntime, ITwaNet)? jsProcessing,
            Guid productId, Guid? positionId = null)
        {
            // Gets an item position
            var item = PersonalInfo.Order.ItemById(productId, positionId);       
            IncreaseAmountOfProduct(item);

            // Calculates a checkin and allowed bonus amount to pay
            if (IsLoyaltyProgramAvailableForProcess)
            {
                await CalculateCheckinAndAllowedSumAsync(jsProcessing);
            }
            else
            {
                await SavePersonalDataOfOrderInServerAsync();
            }

            return item;
        }

        /// <summary>
        /// Increases the number of positions of the selected product item.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        private async Task<IOrderItemProcessing> IncreaseItemPositionForPageWhereSampleItemPositionAreChangedAsync(Guid productId,
            Guid positionId)
        {
            // Gets an item position
            var item = PersonalInfo.Order.ItemById(productId, positionId);
            IncreaseAmountOfProduct(item);
            // Save the action data of this web app in a API server
            await SavePersonalDataOfOrderInServerAsync();
            return item;
        }

        /// <summary>
        /// Increases the number of positions of the selected product item.
        /// </summary>
        /// <returns></returns>
        private IOrderItemProcessing IncreaseItemPositionForPageWhereModifiersAndSizesOfItemPositionAreSelected()
        {
            // Gets a current item position of the order
            var currItemInfo = CurrItemPosition ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                nameof(IncreaseItemPositionForPageWhereModifiersAndSizesOfItemPositionAreSelected), nameof(Exception),
                typeof(CurrItemPosition).FullName!, ExceptionType.Null);
            var item = currItemInfo.GetItem();
            IncreaseAmountOfProduct(item);
            return item;
        }

        /// <summary>
        /// Increases the number of positions of the selected product item.
        /// </summary>
        /// <returns></returns>
        private IOrderItemProcessing IncreaseItemPositionForPageWhereNumberOfItemPositionIsIncreased()
        {
            // Gets a current item position of the order
            var currItemInfo = CurrItemPosition ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                nameof(IncreaseItemPositionForPageWhereNumberOfItemPositionIsIncreased), nameof(Exception),
                typeof(CurrItemPosition).FullName!, ExceptionType.Null);
            var item = currItemInfo.GetItem();
            IncreaseAmountOfProduct(item);
            return item;
        }

        public async Task<(IOrderItemProcessing Item, bool? HasFirstItemBeenAdded)> AddOrIncreaseItemPositionAsync(PageViewType pageView,
            (IJSRuntime, ITwaNet)? jsProcessing = null, Guid? productId = null, Guid? positionId = null)
        {
            var tuple = (pageView, jsProcessing, productId, positionId);

            return tuple switch
            {
                (PageViewType.SelectingProducts, _, Guid itemId, _)
                => await AddOrIncreaseItemPositionForPageWhereProductsAreSelectedAsync(itemId),
                (PageViewType.ShoppingCart, _, Guid itemId, _)
                => (await IncreaseItemPositionForBasketPageAsync(jsProcessing, itemId, positionId), null),
                (PageViewType.ChangingSelectedProductsWithModifiers, _, Guid itemId, Guid _positionId)
                => (await IncreaseItemPositionForPageWhereSampleItemPositionAreChangedAsync(itemId, _positionId), null),
                (PageViewType.SelectingModifiersAndAmountsForProduct, _, _, _)
                => (IncreaseItemPositionForPageWhereModifiersAndSizesOfItemPositionAreSelected(), null),
                (PageViewType.SelectingAmountsForProducts, _, _, _)
                => (IncreaseItemPositionForPageWhereNumberOfItemPositionIsIncreased(), null),
                _ => throw new NotImplementedException()
            };
        }

        public async Task<(IOrderItemProcessing Item, bool? HasFirstItemBeenAdded)> AddOrIncreaseItemPositionAsync(PageViewType pageView,
            (IJSRuntime, ITwaNet)? jsProcessing = null, TransportItemDto? product = null, Guid? positionId = null)
        {
            var tuple = (pageView, jsProcessing, product, positionId);

            return tuple switch
            {
                (PageViewType.SelectingProducts, _, TransportItemDto item, _)
                => await AddOrIncreaseItemPositionForPageWhereProductsAreSelectedAsync(item),
                (PageViewType.ShoppingCart, _, TransportItemDto item, _)
                => (await IncreaseItemPositionForBasketPageAsync(jsProcessing, item.ItemId, positionId), null),
                (PageViewType.ChangingSelectedProductsWithModifiers, _, TransportItemDto item, Guid _positionId)
                => (await IncreaseItemPositionForPageWhereSampleItemPositionAreChangedAsync(item.ItemId, _positionId), null),
                (PageViewType.SelectingModifiersAndAmountsForProduct, _, _, _)
                => (IncreaseItemPositionForPageWhereModifiersAndSizesOfItemPositionAreSelected(), null),
                (PageViewType.SelectingAmountsForProducts, _, _, _)
                => (IncreaseItemPositionForPageWhereNumberOfItemPositionIsIncreased(), null),
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// Decreases the number of positions of the selected product wihtout modifiers and sizes.
        /// Removes the item if the number of positions is equal to zero.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private async Task<(IOrderItemProcessing? Item, bool IsBasketEmpty, bool IsSeveralItemPosition)>
            RemoveOrDecreaseItemPositionForPageWhereProductsAreSelectedAsync(Guid productId)
        {
            // Executes this condition if there are several items of the same item
            if (PersonalInfo.Order.HaveSampleItemPositions(productId))
            {
                // Gets the product by ID and the group ID
                var product = CommonItem.ProductById(productId, CurrentGroupId);
                // Sets the current product for changing
                CurrItemPosition = new(product);
                return (null, false, true);
            }

            // Gets an item position
            var item = PersonalInfo.Order.FirstItemByIdOrDefault(productId);
            if (item is null) return (null, !PersonalInfo.Order.HaveSelectedItems, false);

            bool isRemoved = RemoveOrDecreaseAmountOfProduct(item);
            // Save the action data of this web app in a API server
            await SavePersonalDataOfOrderInServerAsync();

            return isRemoved ? (null, !PersonalInfo.Order.HaveSelectedItems, false) : (item, false, false);
        }

        /// <summary>
        /// Decrease the number of positions of the selected product item.
        /// Removes the item if the number of positions is equal to zero.
        /// Recalculates the checkin with the allowed bonus sum to pay when the loyalty system is used.
        /// Also:
        /// [
        /// If there are several items in the order with the same product ID (as a rule, these are products with modifiers and sizes)
        /// then just sets the current product for the order and returns the 'true'.
        /// In other case, to decrease the number of positions of the remainder product item and removes it
        /// if the number of positions is equal to zero.
        /// ]
        /// </summary>
        /// <param name="jsProcessing"></param>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        private async Task<(IOrderItemProcessing? Item, bool IsBasketEmpty, bool IsSeveralItemPosition)> RemoveOrDecreaseItemPositionForBasketPageAsync(
            (IJSRuntime, ITwaNet)? jsProcessing, Guid productId, Guid positionId)
        {
            // Gets an item position
            var item = PersonalInfo.Order.ItemById(productId, positionId);

            var isRemoved = RemoveOrDecreaseAmountOfProduct(item);
            if (isRemoved && !PersonalInfo.Order.HaveSelectedItems) return (null, true, PersonalInfo.Order.HaveSampleItemPositions(productId));

            // Calculates a checkin and allowed bonus amount to pay
            if (IsLoyaltyProgramAvailableForProcess)
            {
                await CalculateCheckinAndAllowedSumAsync(jsProcessing);
            }
            else
            {
                // Save the action data of this web app in a API server
                await SavePersonalDataOfOrderInServerAsync();
            }

            return (isRemoved ? null : item, false, PersonalInfo.Order.HaveSampleItemPositions(productId));
        }

        /// <summary>
        /// Decreases the number of positions of the selected product item and removes it if the number of positions is equal to zero.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        private async Task<(IOrderItemProcessing? Item, bool IsBasketEmpty, bool IsSeveralItemPosition)>
            RemoveOrDecreaseItemPositionForPageWhereSampleItemPositionAreChangedAsync(Guid productId, Guid positionId)
        {
            // Gets an item position
            var item = PersonalInfo.Order.ItemById(productId, positionId);
            var isRemoved = RemoveOrDecreaseAmountOfProduct(item);
            // Save the action data of this web app in a API server
            await SavePersonalDataOfOrderInServerAsync();

            return (isRemoved ? null : item, !PersonalInfo.Order.HaveSelectedItems,
                PersonalInfo.Order.HaveSampleItemPositions(productId));
        }

        /// <summary>
        /// Decreases the number of positions of the selected product item and removes it if the number of positions is equal to zero.
        /// </summary>
        /// <returns></returns>
        private (IOrderItemProcessing? Item, bool IsBasketEmpty, bool IsSeveralItemPosition)
            RemoveOrDecreaseItemPositionForPageWhereModifiersAndSizesOfItemPositionAreSelected()
        {
            // Gets a current item position of the order
            var currItemInfo = CurrItemPosition ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                nameof(RemoveOrDecreaseItemPositionForPageWhereModifiersAndSizesOfItemPositionAreSelected), nameof(Exception), typeof(CurrItemPosition).FullName!,
                ExceptionType.Null);
            var item = currItemInfo.GetItem();
            return (RemoveOrDecreaseAmountOfProduct(item) ? null : item, !PersonalInfo.Order.HaveSelectedItems,
                PersonalInfo.Order.HaveSampleItemPositions(item.ProductId));
        }

        /// <summary>
        /// Decreases the number of positions of the current product item and removes it if the number of positions is equal to zero.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        private (IOrderItemProcessing? Item, bool IsBasketEmpty, bool IsSeveralItemPosition)
            RemoveOrDecreaseItemPositionForPageWhereNumberOfItemPositionIsIncreased()
        {
            // Gets a current item position of the order
            var currItemInfo = CurrItemPosition ?? throw new InfoException(typeof(OnlineStoreService).FullName!,
                nameof(RemoveOrDecreaseItemPositionForPageWhereNumberOfItemPositionIsIncreased), nameof(Exception),
                typeof(CurrItemPosition).FullName!, ExceptionType.Null);
            var item = currItemInfo.GetItem();
            return (RemoveOrDecreaseAmountOfProduct(item) ? null : item, !PersonalInfo.Order.HaveSelectedItems, false);
        }

        public async Task<(IOrderItemProcessing? Item, bool IsBasketEmpty,
            bool IsSeveralItemPosition)> RemoveOrDecreaseItemPositionAsync(PageViewType pageView,
            (IJSRuntime, ITwaNet)? jsProcessing = null, Guid? productId = null, Guid? positionId = null)
        {
            var tuple = (pageView, jsProcessing, productId, positionId);

            return tuple switch
            {
                (PageViewType.SelectingProducts, _, Guid itemId, _)
                => await RemoveOrDecreaseItemPositionForPageWhereProductsAreSelectedAsync(itemId),
                (PageViewType.ShoppingCart, _, Guid itemId, Guid _positionId)
                => await RemoveOrDecreaseItemPositionForBasketPageAsync(jsProcessing, itemId, _positionId),
                (PageViewType.ChangingSelectedProductsWithModifiers, _, Guid itemId, Guid _positionId)
                => await RemoveOrDecreaseItemPositionForPageWhereSampleItemPositionAreChangedAsync(itemId, _positionId),
                (PageViewType.SelectingModifiersAndAmountsForProduct, _, _, _)
                => RemoveOrDecreaseItemPositionForPageWhereModifiersAndSizesOfItemPositionAreSelected(),
                (PageViewType.SelectingAmountsForProducts, _, _, _)
                => RemoveOrDecreaseItemPositionForPageWhereNumberOfItemPositionIsIncreased(),
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// Decreases the number of positions of the selected product wihtout modifiers and sizes.
        /// Removes the item if the number of positions is equal to zero.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private async Task<(IOrderItemProcessing? Item, bool IsBasketEmpty, bool IsSeveralItemPosition)>
            RemoveOrDecreaseItemPositionForPageWhereProductsAreSelectedAsync(TransportItemDto product)
        {
            if (PersonalInfo.Order.HaveSampleItemPositions(product.ItemId))
            {
                // Sets the current product for changing
                CurrItemPosition = new(product);
                return (null, false, true);
            }

            // Gets an item position
            var item = PersonalInfo.Order.FirstItemByIdOrDefault(product.ItemId);
            if (item is null) return (null, !PersonalInfo.Order.HaveSelectedItems, false);

            bool isRemoved = RemoveOrDecreaseAmountOfProduct(item);
            // Save the action data of this web app in a API server
            await SavePersonalDataOfOrderInServerAsync();

            return isRemoved ? (null, !PersonalInfo.Order.HaveSelectedItems, false) : (item, false, false);
        }

        public async Task<(IOrderItemProcessing? Item, bool IsBasketEmpty, bool IsSeveralItemPosition)>
            RemoveOrDecreaseItemPositionAsync(PageViewType pageView, (IJSRuntime, ITwaNet)? jsProcessing = null,
            TransportItemDto? product = null, Guid? positionId = null)
        {
            var tuple = (pageView, jsProcessing, product, positionId);

            return tuple switch
            {
                (PageViewType.SelectingProducts, _, TransportItemDto item, _)
                => await RemoveOrDecreaseItemPositionForPageWhereProductsAreSelectedAsync(item),
                (PageViewType.ShoppingCart, _, TransportItemDto item, Guid _positionId)
                => await RemoveOrDecreaseItemPositionForBasketPageAsync(jsProcessing, item.ItemId, _positionId),
                (PageViewType.ChangingSelectedProductsWithModifiers, _, TransportItemDto item, Guid _positionId)
                => await RemoveOrDecreaseItemPositionForPageWhereSampleItemPositionAreChangedAsync(item.ItemId, _positionId),
                (PageViewType.SelectingModifiersAndAmountsForProduct, _, _, _)
                => RemoveOrDecreaseItemPositionForPageWhereModifiersAndSizesOfItemPositionAreSelected(),
                (PageViewType.SelectingAmountsForProducts, _, _, _)
                => RemoveOrDecreaseItemPositionForPageWhereNumberOfItemPositionIsIncreased(),
                _ => throw new NotImplementedException()
            };
        }

        public void SetCurrentGroup(Guid groupId)
            => CurrentGroupId = groupId;
    }
}