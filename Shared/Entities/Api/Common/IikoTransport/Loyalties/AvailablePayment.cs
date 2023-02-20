using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.Loyalties
{
    /// <summary>
    /// Marketing campaigns with available payment.
    /// </summary>
    public class AvailablePayment
    {
        /// <summary>
        /// Marketing campaign id
        /// </summary>
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public Guid Id { get; set; }

        /// <summary>
        /// Max sum
        /// </summary>
        [JsonProperty(PropertyName = "maxSum", Required = Required.Always)]
        public double MaxSum { get; set; }

        /// <summary>
        /// Payment order. In case of partial payment, payments with lesser order should be filled first
        /// </summary>
        [JsonProperty(PropertyName = "order", Required = Required.Always)]
        public int Order { get; set; }
    }
}