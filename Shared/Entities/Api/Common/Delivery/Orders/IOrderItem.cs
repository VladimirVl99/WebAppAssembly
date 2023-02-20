namespace WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders
{
    /// <summary>
    /// Contains information about the goods for the order.
    /// The scheme is taken from https://api-ru.iiko.services/#tag/Deliveries:-Create-and-update/paths/~1api~11~1deliveries~1create/post.
    /// </summary>
    public interface IOrderItem
    {
        /// <summary>
        /// Product name.
        /// </summary>
        string ProductName { get; }

        /// <summary>
        /// ID of menu item.
        /// </summary>
        Guid ProductId { get; }

        /// <summary>
        /// Modifiers.
        /// </summary>
        IEnumerable<Modifier> Modifiers { get; }

        /// <summary>
        /// Price per item unit. Can be sent different from the price in the base menu.
        /// </summary>
        double Price { get; }

        /// <summary>
        /// The item's price with the total price of selected modifiers.
        /// </summary>
        double PriceWithSelectedModifiers { get; }

        /// <summary>
        /// The total cost, taking into account the quantity of the selected product and modifiers.
        /// </summary>
        double TotalPrice { get; }

        /// <summary>
        /// The total cost of selected modifiers.
        /// </summary>
        double TotalPriceOfModifiers { get; }

        /// <summary>
        /// Unique identifier of the item in the order. MUST be unique for the whole system.
        /// Therefore it must be generated with Guid.NewGuid().
        /// </summary>
        Guid? PositionId { get; }

        /// <summary>
        /// Item's type.
        /// </summary>
        OrderItemType Type { get; }

        /// <summary>
        /// Quantity.
        /// </summary>
        double Amount { get; }

        /// <summary>
        /// Size ID. Required if a stock list item has a size scale.
        /// </summary>
        Guid? ProductSizeId { get; }

        /// <summary>
        /// Combo details if combo includes order item.
        /// </summary>
        ComboInformation? ComboInformation { get; }

        /// <summary>
        /// Comment.
        /// </summary>
        string? Comment { get; }

        /// <summary>
        /// Modifier groups.
        /// </summary>
        IEnumerable<SimpleGroupModifier>? SimpleGroupModifiers { get; }

        /// <summary>
        /// All modifiers of the item.
        /// </summary>
        IEnumerable<SimpleModifier>? SimpleModifiers { get; }
    }
}