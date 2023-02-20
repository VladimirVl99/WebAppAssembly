using WebAppAssembly.Shared.Entities.OnlineStore.Orders;

namespace WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders
{
    /// <summary>
    /// Information about an order.
    /// </summary>
    public interface IOrder
    {
        /// <summary>
        /// The order's unique ID.
        /// </summary>
        Guid OperationId { get; }

        /// <summary>
        /// The selected items in the order.
        /// </summary>
        ICollection<IOrderItemProcessing> Items { get; }

        /// <summary>
        /// Free items
        /// </summary>
        ICollection<IOrderItemProcessing> FreeItems { get; }

        /// <summary>
        /// A comment on the order.
        /// </summary>
        string? Comment { get; set; }

        /// <summary>
        /// The total amount of the order.
        /// </summary>
        double PaymentAmountOfSeletedItems { get; }

        /// <summary>
        /// The order's created date.
        /// </summary>
        string? CreatedDate { get; }

        /// <summary>
        /// The total amount of selected items in the order.
        /// </summary>
        double NumberOfSelectedItems { get; }

        /// <summary>
        /// A customer's wallet balance.
        /// </summary>
        double WalletBalance { get; }

        /// <summary>
        /// Selected the number of bonuses.
        /// </summary>
        int SelectedNumberOfBonuses { get; }

        /// <summary>
        /// A coupon.
        /// </summary>
        string? Coupon { get; set; }

        /// <summary>
        /// The total amount of the discounts.
        /// </summary>
        double DiscountAmount { get; }

        /// <summary>
        /// The order's total amount with the discounts.
        /// </summary>
        double TotalPaymentAmount { get; }

        /// <summary>
        /// The discount's procent.
        /// </summary>
        double DiscountProcent { get; }

        /// <summary>
        /// Free items that are counted from the same selected items.
        /// </summary>
        ICollection<Guid> DiscountFreeItems { get; }
    }
}
