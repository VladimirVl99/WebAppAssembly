using Newtonsoft.Json;
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
        [JsonProperty(PropertyName = "operationId", Required = Required.Always)]
        Guid OperationId { get; }

        /// <summary>
        /// The selected items in the order.
        /// </summary>
        [JsonProperty(PropertyName = "items", Required = Required.Always)]
        ICollection<IOrderItemProcessing> Items { get; }

        /// <summary>
        /// Free items
        /// </summary>
        [JsonProperty(PropertyName = "freeItems", Required = Required.Always)]
        ICollection<IOrderItemProcessing> FreeItems { get; }

        /// <summary>
        /// A comment on the order.
        /// </summary>
        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)]
        string? Comment { get; set; }

        /// <summary>
        /// The total amount of the order.
        /// </summary>
        [JsonProperty(PropertyName = "paymentAmountOfSelectedItems", Required = Required.Always)]
        double PaymentAmountOfSeletedItems { get; }

        /// <summary>
        /// The order's created date.
        /// </summary>
        [JsonProperty(PropertyName = "createdDate", Required = Required.Always)]
        string? CreatedDate { get; }

        /// <summary>
        /// The total amount of selected items in the order.
        /// </summary>
        [JsonProperty(PropertyName = "numberOfSelectedItems", Required = Required.Always)]
        double NumberOfSelectedItems { get; }

        /// <summary>
        /// A customer's wallet balance.
        /// </summary>
        [JsonProperty(PropertyName = "walletBalance", Required = Required.Always)]
        double WalletBalance { get; }

        /// <summary>
        /// Selected the number of bonuses.
        /// </summary>
        [JsonProperty(PropertyName = "selectedNumberOfBonuses", Required = Required.Always)]
        int SelectedNumberOfBonuses { get; }

        /// <summary>
        /// A coupon.
        /// </summary>
        [JsonProperty(PropertyName = "coupon", DefaultValueHandling = DefaultValueHandling.Ignore)]
        string? Coupon { get; set; }

        /// <summary>
        /// The total amount of the discounts.
        /// </summary>
        [JsonProperty(PropertyName = "discountAmount", Required = Required.Always)]
        double DiscountAmount { get; }

        /// <summary>
        /// The order's total amount with the discounts.
        /// </summary>
        [JsonProperty(PropertyName = "totalPaymentAmount", Required = Required.Always)]
        double TotalPaymentAmount { get; }

        /// <summary>
        /// The discount's procent.
        /// </summary>
        [JsonProperty(PropertyName = "discountProcent", Required = Required.Always)]
        double DiscountProcent { get; }

        /// <summary>
        /// Free items that are counted from the same selected items.
        /// </summary>
        [JsonProperty(PropertyName = "discountFreeItems", Required = Required.Always)]
        ICollection<Guid> DiscountFreeItems { get; }
    }
}
