namespace WebAppAssembly.Shared.Entities.Api.Common.OfTelegram.NativePopupParams
{
    /// <summary>
    /// Describes common information about the native popup.
    /// Source: https://core.telegram.org/bots/webapps#popupparams.
    /// </summary>
    public class PopupParamsShort
    {
        /// <summary>
        /// Optional. The text to be displayed in the popup title, 0-64 characters.
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// The message to be displayed in the body of the popup, 1-256 characters.
        /// </summary>
        public string Message { get; set; } = default!;
    }
}
