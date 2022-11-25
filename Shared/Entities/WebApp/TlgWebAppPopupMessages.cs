using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Entities.WebApp
{
    public class TlgWebAppPopupMessages
    {
        [JsonProperty("loayltyProgramUnavailable")]
        [JsonPropertyName("loayltyProgramUnavailable")]
        public TlgWebAppPopupMessage? LoayltyProgramUnavailable { get; set; }
    }
}
