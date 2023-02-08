using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebAppAssembly.Shared.Entities.CreateDelivery;
using Modifier = WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders.Modifier;

namespace WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders
{
    /// <summary>
    /// Contains information about order items.
    /// </summary>
    [JsonObject]
    public class OrderItem : IOrderItem
    {
        private string? _productName;
        private IEnumerable<Modifier>? _modifiers;


        [JsonProperty(PropertyName = "productName", Required = Required.Always)]
        public string ProductName
        {
            get => _productName ?? "???";
            set => _productName = value;
        }

        [JsonProperty(PropertyName = "productId", Required = Required.Always)]
        public Guid ProductId { get; set; }

        [JsonProperty(PropertyName = "modifiers", Required = Required.Default,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<Modifier> Modifiers
        {
            get => _modifiers ?? Enumerable.Empty<Modifier>();
            set => _modifiers = value;
        }

        [JsonProperty(PropertyName = "price", Required = Required.Always)]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "totalPrice", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double PriceWithSelectedModifiers { get; }

        [JsonProperty(PropertyName = "totalPrice", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double TotalPrice { get; set; }

        [JsonProperty(PropertyName = "totalPriceOfModifiers", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double TotalPriceOfModifiers { get; set; }

        [JsonProperty(PropertyName = "positionId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? PositionId { get; set; }

        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderItemType Type { get; set; }

        [JsonProperty(PropertyName = "amount", Required = Required.Always)]
        public double Amount { get; set; }

        [JsonProperty(PropertyName = "productSizeId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? ProductSizeId { get; set; }

        [JsonProperty(PropertyName = "comboInformation", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ComboInformation? ComboInformation { get; set; }

        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Comment { get; set; }

        [JsonProperty(PropertyName = "simpleGroupModifiers", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<SimpleGroupModifier>? SimpleGroupModifiers { get; set; }

        [JsonProperty(PropertyName = "simpleModifiers", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<SimpleModifier>? SimpleModifiers { get; set; }
    }
}
