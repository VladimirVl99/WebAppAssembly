using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.OfTelegram
{
    /// <summary>
    /// Information about texts for the main button of Telegram in Web App.
    /// Source: https://core.telegram.org/bots/webapps#mainbutton.
    /// </summary>
    public class TgWebAppBtnTxts
    {
        /// <summary>
        /// For a page where's selecting products.
        /// </summary>
        [JsonProperty(PropertyName = "selectingProducts", Required = Required.Always)]
        public string SelectingProducts { get; set; } = default!;

        /// <summary>
        /// For a page where's selecting modifiers for a selected product.
        /// </summary>
        [JsonProperty(PropertyName = "selectingModifiersAndAmountsForProduct",
            Required = Required.Always)]
        public string SelectingModifiersAndAmountsForProduct { get; set; } = default!;

        /// <summary>
        /// For a page where's basket of an order.
        /// </summary>
        [JsonProperty(PropertyName = "shoppingCart", Required = Required.Always)]
        public string ShoppingCart { get; set; } = default!;

        /// <summary>
        /// For a page where's changing quantity of products with modifiers.
        /// </summary>
        [JsonProperty(PropertyName = "changingSelectedProductsWithModifiers", Required = Required.Always)]
        public string ChangingSelectedProductsWithModifiers { get; set; } = default!;

        /// <summary>
        /// For a page where's changing quantity of simple products.
        /// </summary>
        [JsonProperty(PropertyName = "selectingAmountsForProducts", Required = Required.Always)]
        public string SelectingAmountsForProducts { get; set; } = default!;
    }
}
