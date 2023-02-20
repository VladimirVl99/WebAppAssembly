namespace WebAppAssembly.Shared.Entities.Api.Common.OfTelegram.NativePopupParams
{
    /// <summary>
    /// Describes common information about the native popup button.
    /// Source: https://core.telegram.org/bots/webapps#popupbutton.
    /// </summary>
    public class PopupButtonShort
    {
        /// <summary>
        /// Optional. Identifier of the button, 0-64 characters. Set to empty string by default.
        /// If the button is pressed, its id is returned in the callback and the popupClosed event.
        /// </summary>
        public string Id { get; set; } = default!;

        /// <summary>
        /// Optional. The text to be displayed on the button, 0-64 characters.
        /// Required if type is default or destructive. Irrelevant for other types.
        /// </summary>
        public string Text { get; set; } = default!;
    }
}
