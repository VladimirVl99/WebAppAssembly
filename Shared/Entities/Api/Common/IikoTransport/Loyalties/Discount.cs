using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.Loyalties
{
    /// <summary>
    /// Discount operation applied to order items.
    /// </summary>
    public class Discount
    {
        /// <summary>
        /// Enum: 0 1 2 3
        /// Operation Type Code.
        /// 0 - fixed discount for the entire order,
        /// 1 - fixed discount for the item,
        /// 2 - free product,
        /// 3 - other type of discounts
        /// </summary>
        [JsonProperty(PropertyName = "code", Required = Required.Always)]
        public OperationTypeCode Code { get; set; }

        /// <summary>
        /// Id of item the discount is applied to. If null - discount applied to whole orders
        /// </summary>
        [JsonProperty(PropertyName = "orderItemId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? OrderItemId { get; set; }

        /// <summary>
        /// Id of item the discount is applied to. If null - discount applied to whole orders.
        /// </summary>
        [JsonProperty(PropertyName = "positionId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? PositionId { get; set; }

        /// <summary>
        /// Discount sum
        /// </summary>
        [JsonProperty(PropertyName = "discountSum", Required = Required.Always)]
        public double DiscountSum { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        [JsonProperty(PropertyName = "amount", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double Amount { get; set; }

        /// <summary>
        /// Comment. Can be null
        /// </summary>
        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Comment { get; set; }
    }
}
