using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class Group
    {
        [Required]
        [JsonProperty("imageLinks")]
        public IEnumerable<string>? ImageLinks { get; set; }
        [JsonProperty("parentGroup")]
        public Guid? ParentGroup { get; set; }
        [Required]
        [JsonProperty("order")]
        public int Order { get; set; }
        [Required]
        [JsonProperty("isIncludedInMenu")]
        public bool IsIncludedInMenu { get; set; }
        [Required]
        [JsonProperty("isGroupModifier")]
        public bool IsGroupModifier { get; set; }
        [Required]
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("code")]
        public string? Code { get; set; }
        [Required]
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("description")]
        public string? Description { get; set; }
        [JsonProperty("additionalInfo")]
        public string? AdditionalInfo { get; set; }
        [JsonProperty("tags")]
        public IEnumerable<string>? Tags { get; set; }
        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }
        [JsonProperty("seoDescription")]
        public string? SeoDescription { get; set; }
        [JsonProperty("seoText")]
        public string? SeoText { get; set; }
        [JsonProperty("seoKeywords")]
        public string? SeoKeywords { get; set; }
        [JsonProperty("seoTitle")]
        public string? SeoTitle { get; set; }
    }
}
