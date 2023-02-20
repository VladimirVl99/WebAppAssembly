namespace WebAppAssembly.Shared.Entities.Api.Common.OfTelegram.NativePopupParams
{
    /// <summary>
    /// Describes the native popup button.
    /// Source: https://core.telegram.org/bots/webapps#popupbutton.
    /// </summary>
    public class NativePopupButton : PopupButtonShort
    {
        /// <summary>
        /// Optional. Type of the button. Set to default by default.
        /// </summary>
        public string Type { get; set; }

        public NativePopupButton(string id, string text, string type)
        {
            Id = id;
            Type = type;
            Text = text;
        }
    }
}
