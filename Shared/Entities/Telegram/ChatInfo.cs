using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class ChatInfo
    {
        [JsonProperty("chatId")]
        [JsonPropertyName("chatId")]
        public long ChatId { get; set; }
    }
}
