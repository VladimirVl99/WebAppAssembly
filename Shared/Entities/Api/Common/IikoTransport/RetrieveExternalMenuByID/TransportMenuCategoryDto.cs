using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.RetrieveExternalMenuByID
{
    /// <summary>
    /// The menu's categories.
    /// </summary>
    [JsonObject]
    public class TransportMenuCategoryDto
    {
        private string? _name;


        [JsonProperty(PropertyName = "items", Required = Required.Always)]
        public IEnumerable<TransportItemDto> Items { get; set; } = default!;

        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name
        {
            get => string.IsNullOrWhiteSpace(_name) ? "???" : _name;
            set => _name = value;
        }

        [JsonProperty(PropertyName = "description", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Description { get; set; }

        [JsonProperty(PropertyName = "buttonImageUrl", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ButtonImageUrl { get; set; }

        [JsonProperty(PropertyName = "headerImageUrl", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? РeaderImageUrl { get; set; }

        [JsonProperty(PropertyName = "iikoGroupId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? IikoGroupId { get; set; }
    }
}
