using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class Group
    {
        [Required]
        [JsonProperty("imageLinks")]
        [JsonPropertyName("imageLinks")]
        public IEnumerable<string>? ImageLinks { get; set; }
        [JsonProperty("parentGroup")]
        [JsonPropertyName("parentGroup")]
        public Guid? ParentGroup { get; set; }
        [Required]
        [JsonProperty("order")]
        [JsonPropertyName("order")]
        public int Order { get; set; }
        [Required]
        [JsonProperty("isIncludedInMenu")]
        [JsonPropertyName("isIncludedInMenu")]
        public bool IsIncludedInMenu { get; set; }
        [Required]
        [JsonProperty("isGroupModifier")]
        [JsonPropertyName("isGroupModifier")]
        public bool IsGroupModifier { get; set; }
        [Required]
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonProperty("code")]
        [JsonPropertyName("code")]
        public string? Code { get; set; }
        [Required]
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonProperty("additionalInfo")]
        [JsonPropertyName("additionalInfo")]
        public string? AdditionalInfo { get; set; }
        [JsonProperty("tags")]
        [JsonPropertyName("tags")]
        public IEnumerable<string>? Tags { get; set; }
        [JsonProperty("isDeleted")]
        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }
        [JsonProperty("seoDescription")]
        [JsonPropertyName("seoDescription")]
        public string? SeoDescription { get; set; }
        [JsonProperty("seoText")]
        [JsonPropertyName("seoText")]
        public string? SeoText { get; set; }
        [JsonProperty("seoKeywords")]
        [JsonPropertyName("seoKeywords")]
        public string? SeoKeywords { get; set; }
        [JsonProperty("seoTitle")]
        [JsonPropertyName("seoTitle")]
        public string? SeoTitle { get; set; }
    }
}
