namespace WebAppAssembly.Shared.Entities.Api.Common.OfTelegram
{
    /// <summary>
    /// Type can be one of these values:
    /// - error, indicates that a task or action has failed,
    /// - success, indicates that a task or action has completed successfully,
    /// - warning, indicates that a task or action produced a warning.
    /// Source: https://core.telegram.org/bots/webapps#hapticfeedback.
    /// </summary>
    public enum HapticFeedBackNotificationType
    {
        Error,
        Success,
        Warning
    }
}
