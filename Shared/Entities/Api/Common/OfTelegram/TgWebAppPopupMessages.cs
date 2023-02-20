using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.OfTelegram
{
    /// <summary>
    /// Popup messages of the Telegram for the Web App.
    /// </summary>
    [JsonObject]
    public class TgWebAppPopupMessages
    {
        /// <summary>
        /// Unvailable loyalty program.
        /// </summary>
        [JsonProperty(PropertyName = "loayltyProgramUnavailable", Required = Required.Always)]
        public TgWebAppPopupMessage LoayltyProgramUnavailable { get; set; } = default!;

        /// <summary>
        /// Unvailable minimum amount of payment with discounts.
        /// </summary>
        [JsonProperty(PropertyName = "unavailableMinSumWithDiscountForPay", Required = Required.Always)]
        public TgWebAppPopupMessage UnavailableMinSumWithDiscountForPay { get; set; } = default!;

        /// <summary>
        /// Unvailable minimum amount of payment without discounts.
        /// </summary>
        [JsonProperty(PropertyName = "unavailableMinSumtForPay", Required = Required.Always)]
        public TgWebAppPopupMessage UnavailableMinSumtForPay { get; set; } = default!;

        /// <summary>
        /// Incorrectly selected modifiers for a product.
        /// </summary>
        [JsonProperty(PropertyName = "incorrectSelectingOfModifier", Required = Required.Always)]
        public TgWebAppPopupMessage IncorrectSelectedModifier { get; set; } = default!;

        /// <summary>
        /// Incorrect city format.
        /// </summary>
        [JsonProperty(PropertyName = "incorrectCityFormat", Required = Required.Always)]
        public TgWebAppPopupMessage IncorrectCityFormat { get; set; } = default!;

        /// <summary>
        /// Incorrect street format.
        /// </summary>
        [JsonProperty(PropertyName = "incorrectStreetFormat", Required = Required.Always)]
        public TgWebAppPopupMessage IncorrectStreetFormat { get; set; } = default!;

        /// <summary>
        /// Incorrect house format.
        /// </summary>
        [JsonProperty(PropertyName = "incorrectHouseFormat", Required = Required.Always)]
        public TgWebAppPopupMessage IncorrectHouseFormat { get; set; } = default!;

        /// <summary>
        /// The customer wallet balance has been changed.
        /// </summary>
        [JsonProperty(PropertyName = "walletBalanceChangedInIikoBiz", Required = Required.Always)]
        public TgWebAppPopupMessage WalletBalanceChangedInIikoBiz { get; set; } = default!;

        /// <summary>
        /// Empty the basket of an online store.
        /// </summary>
        [JsonProperty(PropertyName = "emptyShoppingCart", Required = Required.Always)]
        public TgWebAppPopupMessage EmptyShoppingCart { get; set; } = default!;

        /// <summary>
        /// Empty selected items.
        /// </summary>
        [JsonProperty(PropertyName = "emptySelectedItemsWithModifiers", Required = Required.Always)]
        public TgWebAppPopupMessage EmptySelectedItemsWithModifiers { get; set; } = default!;
    }
}
