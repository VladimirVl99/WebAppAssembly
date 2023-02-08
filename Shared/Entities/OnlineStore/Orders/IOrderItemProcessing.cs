using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders;

namespace WebAppAssembly.Shared.Entities.OnlineStore.Orders
{
    /// <summary>
    /// Processing of the goods for the order.
    /// </summary>
    public interface IOrderItemProcessing : IOrderItem
    {
        /// <summary>
        /// Inticates whether there are modifiers in this item.
        /// </summary>
        bool HaveModifiers { get; }

        /// <summary>
        /// Returns 'true' if the quantity of item is more than zero.
        /// </summary>
        bool HaveItems { get; }


        /// <summary>
        /// Increments the item's quantity and increases the total price.
        /// Returns the amount by which the price was changed.
        /// </summary>
        /// <returns></returns>
        double IncreaseQuantityAndPrice();

        /// <summary>
        /// Decrements the item's quantity and decreases the total price.
        /// Returns the amount by which the price was changed.
        /// </summary>
        /// <returns></returns>
        double DecrementQuantityWithPrice();

        /// <summary>
        /// Returns 'true' if modifiers are selected.
        /// </summary>
        /// <param name="modifierId"></param>
        /// <param name="modifierGroupId"></param>
        /// <returns></returns>
        bool AreModifierSelected(Guid modifierId, Guid? modifierGroupId = null);

        /// <summary>
        /// Increments the modifier's quantity by ID and increases the total price.
        /// Returns the amount by which the price was changed and flag indicating that
        /// the maximum number of modifiers has been selected.
        /// </summary>
        /// <param name="modifierId"></param>
        /// <param name="modifierGroupId"></param>
        /// <returns></returns>
        (double changedPriceBy, bool isMaxQuantityReached) IncreaseQuantityWithPriceOfModifier(Guid modifierId,
            Guid? modifierGroupId = null);

        /// <summary>
        /// Decrements the modifier's quantity by ID and increases the total price.
        /// Returns the amount by which the price was changed and flag indicating that
        /// the minimum number of modifiers has been selected.
        /// </summary>
        /// <param name="modifierId"></param>
        /// <param name="modifierGroupId"></param> 
        /// <returns></returns>
        (double changedPriceBy, bool isMinQuantityReached) DecreaseQuantityWithPriceOfModifier(Guid modifierId,
            Guid? modifierGroupId = null);

        /// <summary>
        /// Gets selected number of modifiers.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productGroupId"></param>
        /// <returns></returns>
        double AmountOfModifier(Guid id, Guid? productGroupId = null);

        /// <summary>
        /// Gets selected modifiers.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Modifier> SelectedModifiers();

        /// <summary>
        /// Returns 'true' if the maximum number of modifiers in a group is selected.
        /// </summary>
        /// <param name="groupModifierId"></param>
        /// <param name="modifierId"></param>
        /// <returns></returns>
        bool IsMaxAmountOfGroupModifierReached(Guid groupModifierId, Guid modifierId);

        /// <summary>
        /// Returns 'true' if the minimum number of modifiers in a group is selected.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsMinAmountOfGroupModifierReached(Guid id);

        /// <summary>
        /// Returns 'true' if the maximum number of a modifier is selected.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsMaxAmountOfModifierReached(Guid id);

        /// <summary>
        /// Returns 'true' if the minimum number of a modifier is selected.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsMinAmountOfModifierReached(Guid id);

        /// <summary>
        /// Returns 'true' if the minimum number of modifiers in all groups is selected.
        /// </summary>
        /// <returns></returns>
        bool IsMinAmountOfGroupModifiersReached();

        /// <summary>
        /// Returns 'true' if the minimum number among all modifiers is selected.
        /// </summary>
        /// <returns></returns>
        bool IsMinAmountOfModifiersReached();

        /// <summary>
        /// Changes the size and price for the item.
        /// Returns the amount by which the price has changed.
        /// </summary>
        /// <param name="sizeId"></param>
        /// <param name="newPrice"></param>
        /// <returns></returns>
        double ChangeSize(Guid sizeId, float newPrice);
    }
}
