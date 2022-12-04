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
        private string? _selectingProducts;
        [JsonProperty("selectingProducts")]
        [JsonPropertyName("selectingProducts")]
        public string SelectingProducts
        {
            get => _selectingProducts ?? string.Empty;
            set => _selectingProducts = value;
        }
        private string? _selectingModifiersAndAmountsForProduct;
        [JsonProperty("selectingModifiersAndAmountsForProduct")]
        [JsonPropertyName("selectingModifiersAndAmountsForProduct")]
        public string SelectingModifiersAndAmountsForProduct
        {
            get => _selectingModifiersAndAmountsForProduct ?? string.Empty;
            set => _selectingModifiersAndAmountsForProduct = value;
        }
        private string? _shoppingCart;
        [JsonProperty("shoppingCart")]
        [JsonPropertyName("shoppingCart")]
        public string ShoppingCart
        {
            get => _shoppingCart ?? string.Empty;
            set => _shoppingCart = value;
        }
        private string? _changingSelectedProductsWithModifiers;
        [JsonProperty("changingSelectedProductsWithModifiers")]
        [JsonPropertyName("changingSelectedProductsWithModifiers")]
        public string ChangingSelectedProductsWithModifiers
        {
            get => _changingSelectedProductsWithModifiers ?? string.Empty;
            set => _changingSelectedProductsWithModifiers = value;
        }
        private string? _selectingAmountsForProducts;
        [JsonProperty("selectingAmountsForProducts")]
        [JsonPropertyName("selectingAmountsForProducts")]
        public string SelectingAmountsForProducts
        {
            get => _selectingAmountsForProducts ?? string.Empty;
            set => _selectingAmountsForProducts = value;
        }
    }
}
