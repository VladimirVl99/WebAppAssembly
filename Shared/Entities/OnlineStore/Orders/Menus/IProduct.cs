using WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus;

namespace WebAppAssembly.Shared.Entities.OnlineStore.Orders.Menus
{
    /// <summary>
    /// Information about a product.
    /// </summary>
    public interface IProduct
    {
        /// <summary>
        /// Flag for the presence of modifiers in the product.
        /// </summary>
        bool HaveModifiers { get; }

        /// <summary>
        /// Flag for the precence of more than one sizes in the product.
        /// </summary>
        bool HaveSeveralSizes { get; }

        /// <summary>
        /// Flag for the presence of modifiers and more than one sizes in the product.
        /// </summary>
        bool HaveModifiersOrSeveralSizes { get; }


        /// <summary>
        /// Product's image link.
        /// </summary>
        /// <returns></returns>
        string ImageLink();

        /// <summary>
        /// Product's price depends on the size.
        /// Can be null.
        /// </summary>
        /// <param name="sizeId"></param>
        /// <returns></returns>
        float? PriceOrNull(Guid? sizeId = null);

        /// <summary>
        /// Product's price depends on the size.
        /// </summary>
        /// <param name="sizeId"></param>
        /// <returns></returns>
        float Price(Guid? sizeId = null);

        /// <summary>
        /// Weight.
        /// </summary>
        /// <returns></returns>
        float Weight();

        /// <summary>
        /// Fats.
        /// </summary>
        /// <returns></returns>
        float Fats();

        /// <summary>
        /// Proteins.
        /// </summary>
        /// <returns></returns>
        float Proteins();

        /// <summary>
        /// Carbs.
        /// </summary>
        /// <returns></returns>
        float Carbs();

        /// <summary>
        /// Energy.
        /// </summary>
        /// <returns></returns>
        float Energy();

        /// <summary>
        /// Modifier groups.
        /// </summary>
        /// <returns></returns>
        IEnumerable<GroupModifier>? ModifierGroups();
    }
}
