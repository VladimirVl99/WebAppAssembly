namespace WebAppAssembly.Shared.Entities.Api.Common.OfTelegram.NativePopupParams
{
    /// <summary>
    /// Describes the native popup.
    /// Source: https://core.telegram.org/bots/webapps#popupparams.
    /// </summary>
    public class NativePopupParams : PopupParamsShort
    {
        /// <summary>
        /// Optional. List of buttons to be displayed in the popup, 1-3 buttons.
        /// Set to [{“type”:“close”}] by default.
        /// </summary>
        public IEnumerable<NativePopupButton> Buttons { get; set; }


        public NativePopupParams(string title, string message, IEnumerable<NativePopupButton> buttons)
        {
            Title = title;
            Message = message;
            Buttons = buttons;
        }
    }
}
