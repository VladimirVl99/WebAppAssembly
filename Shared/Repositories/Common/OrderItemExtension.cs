using WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders;
using OrderItemRequest = WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders.OrderItem;
using OrderItem = WebAppAssembly.Shared.Entities.OnlineStore.Orders.OrderItem;
using WebAppAssembly.Shared.Entities.OnlineStore.Orders;

namespace WebAppAssembly.Shared.Repositories.Common
{
    public static class OrderItemExtension
    {
        public static ICollection<IOrderItemProcessing> ToOrderItems(this IEnumerable<OrderItemRequest> request)
        {
            var items = new List<IOrderItemProcessing>();
            
            foreach (var item in request)
            {
                items.Add(new OrderItem(item.ProductId, item.ProductName, item.Modifiers, item.Price,
                    item.TotalPrice, item.TotalPriceOfModifiers, item.PositionId, item.Type,
                    item.Amount, item.ProductSizeId, item.ComboInformation, item.Comment,
                    item.SimpleGroupModifiers, item.SimpleModifiers));
            }

            return items;
        }
    }
}
