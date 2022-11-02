using WebAppAssembly.Shared.Entities.EMenu;

namespace WebAppAssembly.Shared.Models.Order
{
    public class GroupWithProducts
    {
        public Guid GroupId { get; set; }
        public IEnumerable<Product>? Products { get; set; }
    }
}