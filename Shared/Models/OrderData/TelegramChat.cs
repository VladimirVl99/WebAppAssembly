using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Models.OrderData
{
    public class TelegramChat
    {
        /// <summary>
        /// The chat's ID via the Telegram.
        /// </summary>
        [JsonProperty("chatId")]
        [JsonPropertyName("chatId")]
        public long ChatId { get; private set; }


        public TelegramChat() { }

        public TelegramChat(long chatId)
        {
            ChatId = chatId;
        }


        /// <summary>
        /// Sets/changes the chat ID.
        /// </summary>
        /// <param name="chatId"></param>
        public void SetChatId(long chatId)
            => ChatId = chatId;
    }
}
