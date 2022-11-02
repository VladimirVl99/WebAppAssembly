using WebAppAssembly.Shared.Entities.EMenu;

namespace WebAppAssembly.Shared.Models.Order
{
    public class WebAppMenu
    {
        public Menu? Menu { get; set; }
        public IEnumerable<Product>? NecessaryProducts { get; set; }
        public IEnumerable<Group>? NecessaryGroups { get; set; }
        public IEnumerable<GroupWithProducts>? GroupsWithProducts { get; set; }
    }
}