using WebAppAssembly.Shared.Entities.Api.Common.OfTelegram.NativePopupParams;

namespace WebAppAssembly.Shared.Entities.Api.Common.OfTelegram
{
    /// <summary>
    /// Describes the native popup button.
    /// Source: https://core.telegram.org/bots/webapps#popupbutton.
    /// </summary>
    public class PopupButton : PopupButtonShort
    {
        /// <summary>
        /// Optional. Type of the button. Set to default by default.
        /// </summary>
        public PopupButtonType Type { get; set; }


        public PopupButton(string id, string text, PopupButtonType type)
        {
            Id = id;
            Type = type;
            Text = text;
        }
    }
}
