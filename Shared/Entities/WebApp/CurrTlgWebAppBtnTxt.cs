using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Entities.WebApp
{
    public class TlgWebAppBtnTxts
    {
        [JsonProperty("selectingProducts")]
        [JsonPropertyName("selectingProducts")]
        public string SelectingProducts
        {
            get => SelectingProducts ?? string.Empty;
            set { }
        }
        [JsonProperty("selectingModifiersAndAmountsForProduct")]
        [JsonPropertyName("selectingModifiersAndAmountsForProduct")]
        public string SelectingModifiersAndAmountsForProduct
        {
            get => SelectingModifiersAndAmountsForProduct ?? string.Empty;
            set { }
        }
        [JsonProperty("shoppingCart")]
        [JsonPropertyName("shoppingCart")]
        public string ShoppingCart
        {
            get => ShoppingCart ?? string.Empty;
            set { }
        }
        [JsonProperty("changingSelectedProductsWithModifiers")]
        [JsonPropertyName("changingSelectedProductsWithModifiers")]
        public string ChangingSelectedProductsWithModifiers
        {
            get => ChangingSelectedProductsWithModifiers ?? string.Empty;
            set { }
        }
        [JsonProperty("selectingAmountsForProducts")]
        [JsonPropertyName("selectingAmountsForProducts")]
        public string SelectingAmountsForProducts
        {
            get => SelectingAmountsForProducts ?? string.Empty;
            set { }
        }
    }
}
