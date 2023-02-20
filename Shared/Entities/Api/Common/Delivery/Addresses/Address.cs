using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.Delivery.Addresses
{
    /// <summary>
    /// Contains information about the customer's address.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// City.
        /// </summary>
        [JsonProperty(PropertyName = "city", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? City { get; set; }

        /// <summary>
        /// Street.
        /// </summary>
        [JsonProperty(PropertyName = "street", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Street { get; set; }

        /// <summary>
        /// House.
        /// </summary>
        [JsonProperty(PropertyName = "house", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? House { get; set; }

        /// <summary>
        /// Apartment.
        /// </summary>
        [JsonProperty(PropertyName = "flat", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Flat { get; set; }

        /// <summary>
        /// Entrance.
        /// </summary>
        [JsonProperty(PropertyName = "entrance", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Entrance { get; set; }

        /// <summary>
        /// Floor.
        /// </summary>
        [JsonProperty(PropertyName = "floor", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Floor { get; set; }
    }
}
