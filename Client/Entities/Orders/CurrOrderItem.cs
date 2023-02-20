using WebAppAssembly.Shared.Entities.Exceptions;
using WebAppAssembly.Shared.Entities.OnlineStore.Orders;
using Product = WebAppAssembly.Shared.Entities.OnlineStore.Orders.Menus.Product;

namespace WebAppAssembly.Client.Entities.Orders
{
    /// <summary>
    /// A current selected order item.
    /// </summary>
    public class CurrOrderItem
    {
        /// <summary>
        /// Common information about a product.
        /// </summary>
        public Product ProductInfo { get; set; }

        /// <summary>
        /// Information about an order item.
        /// </summary>
        public IOrderItemProcessing? Item { get; set; }


        public CurrOrderItem(Product productInfo, IOrderItemProcessing? item = null)
        {
            ProductInfo = productInfo;
            Item = item;
        }


        /// <summary>
        /// Gets an order item data.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public IOrderItemProcessing GetItem() => Item ?? throw new InfoException(typeof(CurrOrderItem).FullName!,
            nameof(GetItem), nameof(Exception), typeof(IOrderItemProcessing).FullName!, ExceptionType.Null);
    }
}
