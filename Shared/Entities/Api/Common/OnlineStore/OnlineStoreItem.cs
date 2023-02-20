using Newtonsoft.Json;
using WebAppAssembly.Shared.Entities.Api.Common.Delivery;
using WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus;
using WebAppAssembly.Shared.Entities.Api.Common.OfTelegram;
using DeliveryTerminal = WebAppAssembly.Shared.Entities.Api.Common.General.Terminals.DeliveryTerminal;
using Product = WebAppAssembly.Shared.Entities.OnlineStore.Orders.Menus.Product;

namespace WebAppAssembly.Shared.Entities.Api.Common.OnlineStore
{
    /// <summary>
    /// Common data for the online store.
    /// </summary>
    public class OnlineStoreItem : IOnlineStoreItem
    {
        [JsonProperty(PropertyName = "itemCategories", Required = Required.Always)]
        public IEnumerable<MenuCategory> MenuCategories { get; set; } = default!;

        [JsonProperty(PropertyName = "menus", Required = Required.Always)]
        public IEnumerable<Product> Menus { get; set; } = default!;

        [JsonProperty(PropertyName = "deliveryTerminals", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<DeliveryTerminal>? DeliveryTerminals { get; set; }

        [JsonProperty(PropertyName = "pickupType", Required = Required.Always)]
        public PickupType PickupType { get; set; }

        [JsonProperty(PropertyName = "useIikoBizProgram", Required = Required.Always)]
        public bool UseIikoBizProgram { get; set; }

        [JsonProperty(PropertyName = "useCoupon", Required = Required.Always)]
        public bool UseCoupon { get; set; }

        [JsonProperty(PropertyName = "useDiscountBalance", Required = Required.Always)]
        public bool UseDiscountBalance { get; set; }

        [JsonProperty(PropertyName = "minPaymentAmountInRubOfTg", Required = Required.Always)]
        public float MinPaymentAmountInRubOfTg { get; set; }

        [JsonProperty(PropertyName = "tlgWebAppBtnTxts", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TgWebAppBtnTxts TgWebAppBtnTxts { get; set; } = default!;

        [JsonProperty(PropertyName = "tlgWebAppPopupMessages", Required = Required.Always)]
        public TgWebAppPopupMessages TgWebAppPopupMessages { get; set; } = default!;

        [JsonProperty(PropertyName = "timeOutForLoyaltyProgramProcessing",
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? TimeOutForLoyaltyProgramProcessing { get; set; }

        [JsonProperty(PropertyName = "tlgMainBtnColor", Required = Required.Always)]
        public string TgMainBtnColor { get; set; } = default!;
    }
}
