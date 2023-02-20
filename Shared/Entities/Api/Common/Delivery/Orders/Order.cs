using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders
{
    /// <summary>
    /// Contains information about the order.
    /// </summary>
    [JsonObject]
    public class Order
    {
        /// <summary>
        /// The order's unique ID.
        /// </summary>
        [JsonProperty(PropertyName = "operationId", Required = Required.Always)]
        public Guid OperationId { get; set; }

        /// <summary>
        /// The selected items in the order.
        /// </summary>
        [JsonProperty(PropertyName = "items", Required = Required.Always)]
        public IEnumerable<OrderItem> Items { get; set; } = default!;

        /// <summary>
        /// Free items
        /// </summary>
        [JsonProperty(PropertyName = "freeItems", Required = Required.Always)]
        public IEnumerable<OrderItem> FreeItems { get; set; } = default!;

        /// <summary>
        /// A comment on the order.
        /// </summary>
        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Comment { get; set; }

        /// <summary>
        /// The total amount of the order.
        /// </summary>
        [JsonProperty(PropertyName = "paymentAmountOfSelectedItems", Required = Required.Always)]
        public double PaymentAmountOfSeletedItems { get; set; }

        /// <summary>
        /// The order's total amount with the discounts.
        /// </summary>
        [JsonProperty(PropertyName = "totalPaymentAmount", Required = Required.Always)]
        public double TotalPaymentAmount { get; set; }

        /// <summary>
        /// The order's created date.
        /// </summary>
        [JsonProperty(PropertyName = "createdDate", Required = Required.Always)]
        public string CreatedDate { get; set; } = default!;

        /// <summary>
        /// The total amount of selected items in the order.
        /// </summary>
        [JsonProperty(PropertyName = "numberOfSelectedItems", Required = Required.Always)]
        public double NumberOfSelectedItems { get; set; }

        /// <summary>
        /// Selected the number of bonuses.
        /// </summary>
        [JsonProperty(PropertyName = "selectedNumberOfBonuses", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? SelectedNumberOfBonuses { get; set; }

        /// <summary>
        /// A coupon.
        /// </summary>
        [JsonProperty(PropertyName = "coupon", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Coupon { get; set; }

        /// <summary>
        /// The total amount of the discounts.
        /// </summary>
        [JsonProperty(PropertyName = "discountAmount", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? DiscountAmount { get; set; }

        /// <summary>
        /// The discount's procent.
        /// </summary>
        [JsonProperty(PropertyName = "discountProcent", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? DiscountProcent { get; set; }

        /// <summary>
        /// Free items that are counted from the same selected items.
        /// </summary>
        [JsonProperty(PropertyName = "discountFreeItems", Required = Required.Default,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<Guid> DiscountFreeItems { get; set; } = default!;
    }
}
