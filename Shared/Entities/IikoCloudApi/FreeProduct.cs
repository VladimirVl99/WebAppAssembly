namespace WebAppAssembly.Shared.Entities.IikoCloudApi
{
    public class FreeProduct
    {
        /// <summary>
        /// Id of action that caused the suggestion
        /// </summary>
        public Guid SourceActionId { get; set; }
        /// <summary>
        /// Description for user. Can be null
        /// </summary>
        public string? DescriptionForUser { get; set; }
        /// <summary>
        /// Products that should be added to order
        /// </summary>
        public IEnumerable<Product>? Products { get; set; }
    }
}
