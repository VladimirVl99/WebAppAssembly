using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Entities.WebApp
{
    public class TlgWebAppPopupMessage
    {
        [JsonProperty("title")]
        [JsonPropertyName("title")]
        public string Title
        {
            get => Title ?? string.Empty;
            set { }
        }
        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description
        {
            get => Description ?? string.Empty;
            set { }
        }
    }
}
