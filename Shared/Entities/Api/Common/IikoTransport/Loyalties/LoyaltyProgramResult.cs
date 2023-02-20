using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.Loyalties
{
    /// <summary>
    /// Loyalty program result.
    /// </summary>
    public class LoyaltyProgramResult
    {
        /// <summary>
        /// Program marketing campaign id
        /// </summary>
        [JsonProperty(PropertyName = "marketingCampaignId", Required = Required.Always)]
        public Guid MarketingCampaignId { get; set; }

        /// <summary>
        /// Program name
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Discount operations applied to order items
        /// </summary>
        [JsonProperty(PropertyName = "discounts", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<Discount>? Discounts { get; set; }

        /// <summary>
        /// Suggested items to add or advices for customer
        /// </summary>
        [JsonProperty(PropertyName = "upsales", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<Upsale>? Upsales { get; set; }

        /// <summary>
        /// Program free products
        /// </summary>
        [JsonProperty(PropertyName = "freeProducts", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<FreeProduct>? FreeProducts { get; set; }

        /// <summary>
        /// Ids of combo specification available in current order
        /// </summary>
        [JsonProperty(PropertyName = "availableComboSpecifications", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<Guid>? AvailableComboSpecifications { get; set; }

        /// <summary>
        /// Partially added combos, available for assembly
        /// </summary>
        [JsonProperty(PropertyName = "availableCombos", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<AvailableCombo>? AvailableCombos { get; set; }

        /// <summary>
        /// Certificate number is required for activation
        /// </summary>
        [JsonProperty(PropertyName = "needToActivateCertificate", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool NeedToActivateCertificate { get; set; }
    }
}
