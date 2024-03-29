﻿using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID
{
    public class ExternalMenuById
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonProperty("pureExternalMenuItemCategories")]
        [JsonPropertyName("pureExternalMenuItemCategories")]
        public IEnumerable<TransportMenuCategoryDto>? ExternalMenuItemCategories { get; set; }
    }
}
