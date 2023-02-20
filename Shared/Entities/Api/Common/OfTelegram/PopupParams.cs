using WebAppAssembly.Shared.Entities.Api.Common.OfTelegram.NativePopupParams;

namespace WebAppAssembly.Shared.Entities.Api.Common.OfTelegram
{
    /// <summary>
    /// Describes the native popup.
    /// Source: https://core.telegram.org/bots/webapps#popupparams.
    /// </summary>
    public class PopupParams : PopupParamsShort
    {
        /// <summary>
        /// Optional. List of buttons to be displayed in the popup, 1-3 buttons.
        /// Set to [{“type”:“close”}] by default.
        /// </summary>
        public IEnumerable<PopupButton> Buttons { get; set; }


        public PopupParams(string title, string message, IEnumerable<PopupButton>? buttons = null)
        {
            Title = title;
            Message = message;

            buttons ??= new List<PopupButton>
            {
                new PopupButton(string.Empty, "Default", PopupButtonType.Default)
            };

            Buttons = buttons;
        }
    }
}
