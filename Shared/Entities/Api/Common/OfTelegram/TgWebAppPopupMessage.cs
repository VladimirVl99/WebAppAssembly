using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.OfTelegram
{
    /// <summary>
    /// Popup message of the Telegram.
    /// Source: https://core.telegram.org/bots/webapps#popupparams.
    /// </summary>
    [JsonObject]
    public class TgWebAppPopupMessage
    {
        /// <summary>
        /// Optional. The text to be displayed in the popup title, 0-64 characters.
        /// </summary>
        [JsonProperty(PropertyName = "title", Required = Required.Always)]
        public string Title { get; set; } = default!;

        /// <summary>
        /// The message to be displayed in the body of the popup, 1-256 characters.
        /// </summary>
        [JsonProperty(PropertyName = "description", Required = Required.Always)]
        public string Description { get; set; } = default!;
    }
}
