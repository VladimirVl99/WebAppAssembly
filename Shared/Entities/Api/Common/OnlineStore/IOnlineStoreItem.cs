using WebAppAssembly.Shared.Entities.Api.Common.Delivery;
using WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus;
using WebAppAssembly.Shared.Entities.Api.Common.OfTelegram;
using DeliveryTerminal = WebAppAssembly.Shared.Entities.Api.Common.General.Terminals.DeliveryTerminal;
using Product = WebAppAssembly.Shared.Entities.OnlineStore.Orders.Menus.Product;

namespace WebAppAssembly.Shared.Entities.Api.Common.OnlineStore
{
    /// <summary>
    /// Common data for the online store.
    /// </summary>
    public interface IOnlineStoreItem
    {
        /// <summary>
        /// Menu categories.
        /// </summary>
        IEnumerable<MenuCategory> MenuCategories { get; }

        /// <summary>
        /// Menus.
        /// </summary>
        IEnumerable<Product> Menus { get; }

        /// <summary>
        /// Pickup terminals.
        /// </summary>
        IEnumerable<DeliveryTerminal>? DeliveryTerminals { get; }

        /// <summary>
        /// Shopping methods.
        /// </summary>
        PickupType PickupType { get; }

        /// <summary>
        /// Using iikoBiz for the order.
        /// </summary>
        bool UseIikoBizProgram { get; }

        /// <summary>
        /// Using coupons for the order.
        /// </summary>
        bool UseCoupon { get; }

        /// <summary>
        /// Using discount balance for the order.
        /// </summary>
        bool UseDiscountBalance { get; }

        /// <summary>
        /// The minimum payment amount in rubles in the Telegram.
        /// </summary>
        float MinPaymentAmountInRubOfTg { get; }

        /// <summary>
        /// Texts of the Telegram's main button.
        /// Source: https://core.telegram.org/bots/webapps#mainbutton.
        /// </summary>
        TgWebAppBtnTxts TgWebAppBtnTxts { get; }

        /// <summary>
        /// Messages of the Telegram popups.
        /// Source: https://core.telegram.org/bots/webapps#popupparams.
        /// </summary>
        TgWebAppPopupMessages TgWebAppPopupMessages { get; }

        /// <summary>
        /// Timeout for loyalty program calculation.
        /// </summary>
        double? TimeOutForLoyaltyProgramProcessing { get; }

        /// <summary>
        /// Color of the Telegram's main button.
        /// </summary>
        string TgMainBtnColor { get; }
    }
}
