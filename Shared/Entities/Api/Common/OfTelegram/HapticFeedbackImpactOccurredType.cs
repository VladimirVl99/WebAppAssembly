namespace WebAppAssembly.Shared.Entities.Api.Common.OfTelegram
{
    /// <summary>
    /// Style can be one of these values:
    /// - light, indicates a collision between small or lightweight UI objects,
    /// - medium, indicates a collision between medium-sized or medium-weight UI objects,
    /// - heavy, indicates a collision between large or heavyweight UI objects,
    /// - rigid, indicates a collision between hard or inflexible UI objects,
    /// - soft, indicates a collision between soft or flexible UI objects.
    /// Source: https://core.telegram.org/bots/webapps#hapticfeedback.
    /// </summary>
    public enum HapticFeedbackImpactOccurredType
    {
        Light,
        Medium,
        Heavy,
        Rigid,
        Soft
    }
}
