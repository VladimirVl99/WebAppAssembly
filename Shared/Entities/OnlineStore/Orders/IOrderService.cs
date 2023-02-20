using WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders;
using WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus;

namespace WebAppAssembly.Shared.Entities.OnlineStore.Orders
{
    /// <summary>
    /// Processing the orders.
    /// </summary>
    public interface IOrderService : IOrder
    {
        /// <summary>
        /// Returns 'true' if there are any selected items in the basket.
        /// </summary>
        bool HaveSelectedItems
            => Items.Any();


        /// <summary>
        /// Resets all discount items.
        /// </summary>
        void ResetDiscountItems();

        /// <summary>
        /// Resets discount items except wallets.
        /// </summary>
        void ResetWalletAmounts();

        /// <summary>
        /// Resets discount items except wallets.
        /// </summary>
        void ResetDiscountItemsWithDiscountAmounts();

        /// <summary>
        /// Sets/changes the discount's amount, calculates the discount procent and recalculates the order's payment amount.
        /// </summary>
        /// <param name="amount"></param>
        void SetDiscountAmount(double amount);

        /// <summary>
        /// Sets/changes the discount's free items.
        /// </summary>
        /// <param name="guids"></param>
        void SetDiscountFreeItems(List<Guid> guids);

        /// <summary>
        /// Sets/changes free items.
        /// </summary>
        /// <param name="items"></param>
        void SetFreeItems(List<IOrderItemProcessing> items);

        /// <summary>
        /// Sets the order's payment amount without any discounts.
        /// </summary>
        void ResetFinalPaymentAmount();

        /// <summary>
        /// Resets the order's payment amount and the number of selected items to zero.
        /// </summary>
        void ResetTotalPaymentAmount();

        /// <summary>
        /// Clear the order's basket.
        /// </summary>
        void ClearBasketOfOrder();

        /// <summary>
        /// Sets/changes the wallet's selected amount in the order.
        /// </summary>
        /// <param name="amount"></param>
        void SetSelectedWalletAmount(int amount);

        /// <summary>
        /// Sets/changes the wallet's balance of a customer.
        /// </summary>
        /// <param name="walletBalance"></param>
        void SetWalletBalance(double walletBalance);

        /// <summary>
        /// Gets the total number of the selected items by item ID.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        double TotalNumberOfSelectedItemsById(Guid itemId);

        /// <summary>
        /// Gets the order's payment amount with selected wallet amount.
        /// </summary>
        /// <returns></returns>
        double FinalPaymentAmountWithSelectedWalletAmount();

        /// <summary>
        /// Increases the total number of selected items and increases the total payment amount of the order.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        double IncreaseTotalNumberOfItemAndTotalPaymentAmount(IOrderItemProcessing item);

        /// <summary>
        /// Decreases the total number of selected items and decreases the total payment amount of the order.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        double DecreaseTotalNumberOfItemAndTotalPaymentAmount(IOrderItemProcessing item);

        /// <summary>
        /// Increases the total number of selected modifier of selected item and increases the total
        /// payment amount of the order.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="modifierId"></param>
        /// <param name="modifierGroupId"></param>
        /// <returns></returns>
        double IncreaseTotalNumberOfModifierAndTotalPaymentAmount(IOrderItemProcessing item, Guid modifierId, Guid? modifierGroupId);

        /// <summary>
        /// Decreases the total number of selected modifier of selected item and decreases the total
        /// payment amount of the order.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="modifierId"></param>
        /// <param name="modifierGroupId"></param>
        /// <returns></returns>
        double DecreaseTotalNumberOfModifierAndTotalPaymentAmount(IOrderItemProcessing item, Guid modifierId, Guid? modifierGroupId);

        /// <summary>
        /// Removes the item from the basket.
        /// </summary>
        /// <param name="item"></param>
        void ZeroAmountOfItem(IOrderItemProcessing item);

        /// <summary>
        /// Removes items by the same ID from the basket.
        /// </summary>
        /// <param name="productId"></param>
        void RemoveItemsById(Guid productId);

        /// <summary>
        /// Increases the total payment amount of the order.
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        double IncreaseTotalPaymentAmount(double sum);

        /// <summary>
        /// Decreases the total payment amount of the order.
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        double DecreaseTotalPaymentAmount(double sum);

        /// <summary>
        /// Finds the item by ID.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        IOrderItemProcessing FirstItemById(Guid productId);

        /// <summary>
        /// Tries to find the item by ID or returns NULL.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        IOrderItemProcessing? FirstItemByIdOrDefault(Guid productId);

        /// <summary>
        /// Finds the item by product and position IDs.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        IOrderItemProcessing ItemById(Guid productId, Guid? positionId = null);

        /// <summary>
        /// Returns 'true' if there are sample items by product ID.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        bool HaveSampleItemPositions(Guid productId);

        /// <summary>
        /// Changes the item's size with prices.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="priceOfItemSize"></param>
        void ChangeSizeOfItem(IOrderItemProcessing item, float priceOfItemSize, Guid sizeId);

        /// <summary>
        /// Adds the new item position to the basket.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        IOrderItemProcessing AddItemWithNewPosition(Product product, Guid? sizeId = null);
    }
}
