using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.PersonalData
{
    /// <summary>
    /// Contains information about the Telegram's chat data.
    /// </summary>
    [JsonObject]
    public class TgChat
    {
        /// <summary>
        /// The chat's ID via Telegram.
        /// </summary>
        [JsonProperty(PropertyName = "chatId", Required = Required.Always)]
        public long ChatId { get; set; }
    }
}
