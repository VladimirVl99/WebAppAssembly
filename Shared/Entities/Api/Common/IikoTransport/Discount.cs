using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport
{
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
        [JsonProperty("code")]
        [JsonPropertyName("code")]
        public int Code { get; set; }
        /// <summary>
        /// Id of item the discount is applied to. If null - discount applied to whole orders
        /// </summary>
        [JsonProperty("orderItemId")]
        [JsonPropertyName("orderItemId")]
        public Guid? OrderItemId { get; set; }
        /// <summary>
        /// Discount sum
        /// </summary>
        [JsonProperty("discountSum")]
        [JsonPropertyName("discountSum")]
        public double DiscountSum { get; set; }
        /// <summary>
        /// Amount
        /// </summary>
        [JsonProperty("amount")]
        [JsonPropertyName("amount")]
        public double Amount { get; set; }
        /// <summary>
        /// Comment. Can be null
        /// </summary>
        [JsonProperty("comment")]
        [JsonPropertyName("comment")]
        public string? Comment { get; set; }
    }
}
