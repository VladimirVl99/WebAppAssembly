using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.Loylties
{
    /// <summary>
    /// A customer's wallet balance.
    /// </summary>
    public class WalletBalance
    {
        /// <summary>
        /// Balance.
        /// </summary>
        [JsonProperty(PropertyName = "balance", Required = Required.Always)]
        public double Balance { get; set; }
    }
}
