using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppAssembly.Shared.Entities.Telegram;

namespace TlgWebAppNet
{
    /// <summary>
    /// An interface that facilitates working with the Web App for Telegram Bots API.
    /// Source: https://core.telegram.org/bots/webapps.
    /// </summary>
    public interface ITwaNet
    {
        /// <summary>
        /// Telegram's chat ID.
        /// </summary>
        long ChatId { get; }


        /// <summary>
        /// Sets specified text for Telegram's main button.
        /// Source: https://core.telegram.org/bots/webapps#mainbutton.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="txt">Current button text. Set to CONTINUE by default.</param>
        /// <returns></returns>
        Task SetMainButtonTextAsync(IJSRuntime jsRuntime, string txt);

        /// <summary>
        /// Bot API 6.1+.
        /// A method tells that the user has changed a selection. The Telegram app may play the appropriate haptics.
        /// Do not use this feedback when the user makes or confirms a selection; use it only when the selection changes.
        /// Source: https://core.telegram.org/bots/webapps#hapticfeedback.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <returns></returns>
        Task HapticFeedbackSelectionChangedAsync(IJSRuntime jsRuntime);

        /// <summary>
        /// Bot API 6.1+.
        /// Makes the back button active and visible.
        /// Source: https://core.telegram.org/bots/webapps#backbutton.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <returns></returns>
        Task ShowBackButtonAsync(IJSRuntime jsRuntime);

        /// <summary>
        /// Bot API 6.1+.
        /// Hides the back button.
        /// Source: https://core.telegram.org/bots/webapps#backbutton.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <returns></returns>
        Task HideBackButtonAsync(IJSRuntime jsRuntime);

        /// <summary>
        /// Hides the main button.
        /// Source: https://core.telegram.org/bots/webapps#mainbutton.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <returns></returns>
        Task HideMainButtonAsync(IJSRuntime jsRuntime);

        /// <summary>
        /// Bot API 6.2+.
        /// Shows a native popup described by the 'OK' param argument of the type PopupParams.
        /// The Web App will receive the event popupClosed when the popup is closed or pressed 'OK' button.
        /// Source: https://core.telegram.org/bots/webapps#initializing-web-apps.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="title">Optional. The text to be displayed in the popup title, 0-64 characters.</param>
        /// <param name="description">The message to be displayed in the body of the popup, 1-256 characters.</param>
        /// <param name="notificationType">Notification type of haptic feedback.</param>
        /// <returns></returns>
        Task ShowOkPopupMessageAsync(IJSRuntime jsRuntime, string title, string description, HapticFeedBackNotificationType notificationType);

        /// <summary>
        /// Shows a loading indicator on the button.
        /// It is recommended to display loading progress if the action tied to the button may take a long time.
        /// By default, the button is disabled while the action is in progress.
        /// If the parameter leaveActive=true is passed, the button remains enabled.
        /// Source: https://core.telegram.org/bots/webapps#mainbutton.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="leaveActive">If 'true' is passed, the button remains enabled.</param>
        /// <returns></returns>
        Task ShowProgressAsync(IJSRuntime jsRuntime, bool leaveActive = false);

        /// <summary>
        /// Hides the loading indicator.
        /// Source: https://core.telegram.org/bots/webapps#mainbutton.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <returns></returns>
        Task HideProgressAsync(IJSRuntime jsRuntime);

        /// <summary>
        /// Closes the Web App.
        /// Source: https://core.telegram.org/bots/webapps#initializing-web-apps.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <returns></returns>
        Task CloseWebAppAsync(IJSRuntime jsRuntime);

        /// <summary>
        /// Occurs when the main button is pressed.
        /// Source: https://core.telegram.org/bots/webapps#events-available-for-web-apps,
        /// https://core.telegram.org/bots/webapps#mainbutton.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <returns></returns>
        Task ListenMainButtonAsync(IJSRuntime jsRuntime);

        /// <summary>
        /// Bot API 6.1+.
        /// Occurs when the back button is pressed.
        /// Source: https://core.telegram.org/bots/webapps#events-available-for-web-apps,
        /// https://core.telegram.org/bots/webapps#mainbutton.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <returns></returns>
        Task ListenBackButtonAsync(IJSRuntime jsRuntime);

        /// <summary>
        /// Sets specified color for Telegram's main button.
        /// Source: https://core.telegram.org/bots/webapps#themeparams.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="color">Color for the main button.</param>
        /// <returns></returns>
        Task SetMainBtnColorAsync(IJSRuntime jsRuntime, string color);

        /// <summary>
        /// Bot API 6.1+.
        /// Opens an invoice using the link url. The Web App will receive the event invoiceClosed when the invoice is closed.
        /// If an optional callback parameter was passed, the callback function will be called and the invoice status will
        /// be passed as the first argument.
        /// Occurs when the opened invoice is closed.
        /// eventHandler receives an object with the two fields: url – invoice link provided
        /// and status – one of the invoice statuses:
        /// - paid – invoice was paid successfully,
        /// - cancelled – user closed this invoice without paying,
        /// - failed – user tried to pay, but the payment was failed,
        /// - pending – the payment is still processing. The bot will receive a service message about
        /// a successful payment when the payment is successfully paid.
        /// Source: https://core.telegram.org/bots/webapps#events-available-for-web-apps,
        /// https://core.telegram.org/bots/webapps#initializing-web-apps.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="invoiceLink">Link url.</param>
        /// <returns></returns>
        Task<InvoiceClosedStatus> InvoiceClosedHandlerAsync(IJSRuntime jsRuntime, string invoiceLink);

        /// <summary>
        /// Bot API 6.1+.
        /// A method tells that an impact occurred. The Telegram app may play the appropriate haptics based on style value passed.
        /// Style can be one of these values:
        /// - light, indicates a collision between small or lightweight UI objects,
        /// - medium, indicates a collision between medium-sized or medium-weight UI objects,
        /// - heavy, indicates a collision between large or heavyweight UI objects,
        /// - rigid, indicates a collision between hard or inflexible UI objects,
        /// - soft, indicates a collision between soft or flexible UI objects.
        /// Source: https://core.telegram.org/bots/webapps#hapticfeedback.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="style">Style of haptic feedback.</param>
        /// <returns></returns>
        Task HapticFeedbackImpactOccurredAsync(IJSRuntime jsRuntime, HapticFeedbackImpactOccurredType style);

        /// <summary>
        /// Bot API 6.1+.
        /// A method tells that a task or action has succeeded, failed, or produced a warning.
        /// The Telegram app may play the appropriate haptics based on type value passed. Type can be one of these values:
        /// - error, indicates that a task or action has failed,
        /// - success, indicates that a task or action has completed successfully,
        /// - warning, indicates that a task or action produced a warning.
        /// Source: https://core.telegram.org/bots/webapps#hapticfeedback.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="type">Notification type of haptic feedback.</param>
        /// <returns></returns>
        Task HapticFeedbackNotificationAsync(IJSRuntime jsRuntime, HapticFeedBackNotificationType type);

        /// <summary>
        /// Bot API 6.2+.
        /// A method that shows a native popup described by the params argument of the type PopupParams.
        /// The Web App will receive the event popupClosed when the popup is closed. If an optional callback parameter was passed,
        /// the callback function will be called and the field id of the pressed button will be passed as the first argument.
        /// Source: https://core.telegram.org/bots/webapps#initializing-web-apps,
        /// https://core.telegram.org/bots/webapps#events-available-for-web-apps.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="popup">Describes the native popup. Source: https://core.telegram.org/bots/webapps#popupparams.</param>
        /// <param name="type">Notification type of haptic feedback.</param>
        /// <param name="style">Style of haptic feedback.</param>
        /// <returns></returns>
        Task<string> ShowPopupParamsAsync(IJSRuntime jsRuntime, PopupParams popup, HapticFeedBackNotificationType? type = null,
            HapticFeedbackImpactOccurredType? style = null);
    }
}
