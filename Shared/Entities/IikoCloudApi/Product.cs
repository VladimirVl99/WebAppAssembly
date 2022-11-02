namespace WebAppAssembly.Shared.Entities.IikoCloudApi
{
    public class Product
    {
        /// <summary>
        /// Id of product
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Code of product. Can be null
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Sizes available for that product
        /// </summary>
        public IEnumerable<string>? Size { get; set; }
    }
}
