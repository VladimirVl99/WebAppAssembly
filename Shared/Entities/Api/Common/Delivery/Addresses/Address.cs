using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Entities.Api.Common.Delivery.Addresses
{
    /// <summary>
    /// Contains information about the customer's address.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "city", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "street", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Street { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "house", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? House { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "flat", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Flat { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "entrance", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Entrance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "floor", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Floor { get; set; }
    }
}
