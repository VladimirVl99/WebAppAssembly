namespace WebAppAssembly.Shared.Entities.Api.Common.OfTelegram
{
    /// <summary>
    /// Can be one of these values:
    /// - default, a button with the default style,
    /// - ok, a button with the localized text “OK”,
    /// - close, a button with the localized text “Close”,
    /// - cancel, a button with the localized text “Cancel”,
    /// - destructive, a button with a style that indicates a destructive action(e.g. “Remove”, “Delete”, etc.).
    /// Source: https://core.telegram.org/bots/webapps#popupbutton.
    /// </summary>
    public enum PopupButtonType
    {
        Default,
        Ok,
        Close,
        Cancel,
        Destructive
    }
}
