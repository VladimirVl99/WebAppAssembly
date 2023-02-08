using WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.RetrieveExternalMenuByID;
using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.Entities.WebApp;
using DeliveryTerminal = WebAppAssembly.Shared.Entities.Api.Common.General.Terminals.DeliveryTerminal;

namespace WebAppAssembly.Shared.Entities.Api.Common.OnlineStore
{
    public interface IOnlineStoreItem
    {
        /// <summary>
        /// Menu categories.
        /// </summary>
        IEnumerable<TransportMenuCategoryDto> MenuCategories { get; }

        /// <summary>
        /// Menus.
        /// </summary>
        IEnumerable<TransportItemDto> Menus { get; }

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
        TlgWebAppBtnTxts TgWebAppBtnTxts { get; }

        /// <summary>
        /// Messages of the Telegram popups.
        /// Source: https://core.telegram.org/bots/webapps#popupparams.
        /// </summary>
        TlgWebAppPopupMessages TgWebAppPopupMessages { get; }

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
