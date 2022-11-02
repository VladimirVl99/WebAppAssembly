namespace WebAppAssembly.Shared.Models.Order
{
    public class ProductModel
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int Amount { get; set; }
        public double Price { get; set; }
    }
}
