using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.IikoCloudApi
{
    public class LoyaltyProgramResult
    {
        /// <summary>
        /// Program marketing campaign id
        /// </summary>
        [JsonProperty("marketingCampaignId")]
        [JsonPropertyName("marketingCampaignId")]
        public Guid MarketingCampaignId { get; set; }
        /// <summary>
        /// Program name
        /// </summary>
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        /// <summary>
        /// Discount operations applied to order items
        /// </summary>
        [JsonProperty("discount")]
        [JsonPropertyName("discount")]
        public IEnumerable<Discount>? Discounts { get; set; }
        /// <summary>
        /// Suggested items to add or advices for customer
        /// </summary>
        [JsonProperty("upsales")]
        [JsonPropertyName("upsales")]
        public IEnumerable<Upsale>? Upsales { get; set; }
        /// <summary>
        /// Program free products
        /// </summary>
        [JsonProperty("freeProducts")]
        [JsonPropertyName("freeProducts")]
        public IEnumerable<FreeProduct>? FreeProducts { get; set; }
        /// <summary>
        /// Ids of combo specification available in current order
        /// </summary>
        [JsonProperty("availableComboSpecifications")]
        [JsonPropertyName("availableComboSpecifications")]
        public IEnumerable<Guid>? AvailableComboSpecifications { get; set; }
        /// <summary>
        /// Partially added combos, available for assembly
        /// </summary>
        [JsonProperty("availableCombos")]
        [JsonPropertyName("availableCombos")]
        public IEnumerable<AvailableCombo>? AvailableCombos { get; set; }
        /// <summary>
        /// Certificate number is required for activation
        /// </summary>
        [JsonProperty("needToActivateCertificate")]
        [JsonPropertyName("needToActivateCertificate")]
        public bool NeedToActivateCertificate { get; set; }
    }
}
