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
        private string? _title;
        [JsonProperty("title")]
        [JsonPropertyName("title")]
        public string Title
        {
            get => _title ?? string.Empty;
            set => _title = value;
        }
        private string? _description;
        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description
        {
            get => _description ?? string.Empty;
            set => _description = value;
        }
    }
}
