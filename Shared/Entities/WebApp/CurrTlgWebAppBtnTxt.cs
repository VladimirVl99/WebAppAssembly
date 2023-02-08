using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.WebApp
{
    public class TlgWebAppBtnTxts
    {
        private const string DefaultSelectingProducts = "";
        private const string DefaultSelectingModifiersAndAmountsForProduct = "";
        private const string DefaultShoppingCart = "";
        private const string DefaultChangingSelectedProductsWithModifiers = "";
        private const string DefaultSelectingAmountsForProducts = "";


        private string? _selectingProducts;
        private string? _selectingModifiersAndAmountsForProduct;
        private string? _shoppingCart;
        private string? _changingSelectedProductsWithModifiers;
        private string? _selectingAmountsForProducts;


        [JsonProperty(PropertyName = "selectingProducts", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string SelectingProducts
        {
            get => _selectingProducts ?? DefaultSelectingProducts;
            set => _selectingProducts = value;
        }

        [JsonProperty(PropertyName = "selectingModifiersAndAmountsForProduct",
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string SelectingModifiersAndAmountsForProduct
        {
            get => _selectingModifiersAndAmountsForProduct ?? DefaultSelectingModifiersAndAmountsForProduct;
            set => _selectingModifiersAndAmountsForProduct = value;
        }

        [JsonProperty(PropertyName = "shoppingCart", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ShoppingCart
        {
            get => _shoppingCart ?? DefaultShoppingCart;
            set => _shoppingCart = value;
        }

        [JsonProperty(PropertyName = "changingSelectedProductsWithModifiers",
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ChangingSelectedProductsWithModifiers
        {
            get => _changingSelectedProductsWithModifiers ?? DefaultChangingSelectedProductsWithModifiers;
            set => _changingSelectedProductsWithModifiers = value;
        }

        [JsonProperty(PropertyName = "selectingAmountsForProducts", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string SelectingAmountsForProducts
        {
            get => _selectingAmountsForProducts ?? DefaultSelectingAmountsForProducts;
            set => _selectingAmountsForProducts = value;
        }
    }
}
