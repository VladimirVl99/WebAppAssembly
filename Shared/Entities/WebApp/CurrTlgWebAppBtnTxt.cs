using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Entities.WebApp
{
    public class TlgWebAppBtnTxts
    {
        [JsonProperty("selectingProducts")]
        [JsonPropertyName("selectingProducts")]
        public string SelectingProducts { get; set; }
        [JsonProperty("selectingModifiersAndAmountsForProduct")]
        [JsonPropertyName("selectingModifiersAndAmountsForProduct")]
        public string SelectingModifiersAndAmountsForProduct { get; set; }
    }
}
