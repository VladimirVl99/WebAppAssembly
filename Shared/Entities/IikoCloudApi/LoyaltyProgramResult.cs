namespace WebAppAssembly.Shared.Entities.IikoCloudApi
{
    public class LoyaltyProgramResult
    {
        /// <summary>
        /// Program marketing campaign id
        /// </summary>
        public Guid MarketingCampaignId { get; set; }
        /// <summary>
        /// Program name
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Discount operations applied to order items
        /// </summary>
        public IEnumerable<Discount>? Discounts { get; set; }
        /// <summary>
        /// Suggested items to add or advices for customer
        /// </summary>
        public IEnumerable<Upsale>? Upsales { get; set; }
        /// <summary>
        /// Program free products
        /// </summary>
        public IEnumerable<FreeProduct>? FreeProducts { get; set; }
        /// <summary>
        /// Ids of combo specification available in current order
        /// </summary>
        public IEnumerable<Guid>? AvailableComboSpecifications { get; set; }
        /// <summary>
        /// Partially added combos, available for assembly
        /// </summary>
        public IEnumerable<AvailableCombo>? AvailableCombos { get; set; }
        /// <summary>
        /// Certificate number is required for activation
        /// </summary>
        public bool NeedToActivateCertificate { get; set; }
    }
}
