using Newtonsoft.Json;
using System.Text.Json.Serialization;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class Product
    {
        public Product()
        {
            var id = Guid.NewGuid();
            RemoveButtonId = $"rbutt_{id}";
            AddButtonId = $"abutt_{id}";
            RemoveButtonTextId = $"rbutt_text_{id}";
            AddButtonTextId = $"abutt_text_{id}";
        }

        public Guid OrganizationId { get; set; }
        [JsonProperty("fatAmount")]
        [JsonPropertyName("fatAmount")]
        public double? FatAmount { get; set; }
        [JsonProperty("proteinsAmount")]
        [JsonPropertyName("proteinsAmount")]
        public double? ProteinsAmount { get; set; }
        [JsonProperty("carbohydratesAmount")]
        [JsonPropertyName("carbohydratesAmount")]
        public double? CarbohydratesAmount { get; set; }
        [JsonProperty("energyAmount")]
        [JsonPropertyName("energyAmount")]
        public double? EnergyAmount { get; set; }
        [JsonProperty("fatFullAmount")]
        [JsonPropertyName("fatFullAmount")]
        public double? FatFullAmount { get; set; }
        [JsonProperty("proteinsFullAmount")]
        [JsonPropertyName("proteinsFullAmount")]
        public double? ProteinsFullAmount { get; set; }
        [JsonProperty("carbohydratesFullAmount")]
        [JsonPropertyName("carbohydratesFullAmount")]
        public double? CarbohydratesFullAmount { get; set; }
        [JsonProperty("energyFullAmount")]
        [JsonPropertyName("energyFullAmount")]
        public double? EnergyFullAmount { get; set; }
        [JsonProperty("weight")]
        [JsonPropertyName("weight")]
        public double? Weight { get; set; }
        [JsonProperty("groupId")]
        [JsonPropertyName("groupId")]
        public Guid? GroupId { get; set; }
        [JsonProperty("productCategoryId")]
        [JsonPropertyName("productCategoryId")]
        public Guid? ProductCategoryId { get; set; }
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonProperty("orderItemType")]
        [JsonPropertyName("orderItemType")]
        public string? OrderItemType { get; set; }
        [JsonProperty("modifierSchemaId")]
        [JsonPropertyName("modifierSchemaId")]
        public Guid? ModifierSchemaId { get; set; }
        [JsonProperty("modifierSchemaName")]
        [JsonPropertyName("modifierSchemaName")]
        public string? ModifierSchemaName { get; set; }
        [JsonRequired]
        [JsonProperty("splittable")]
        [JsonPropertyName("splittable")]
        public bool Splittable { get; set; }
        [JsonProperty("measureUnit")]
        [JsonPropertyName("measureUnit")]
        public string? MeasureUnit { get; set; }
        [JsonProperty("sizePrices")]
        [JsonPropertyName("sizePrices")]
        public IEnumerable<SizePrice>? SizePrices { get; set; }
        [JsonProperty("modifiers")]
        [JsonPropertyName("modifiers")]
        public IEnumerable<Modifier>? Modifiers { get; set; }
        [JsonProperty("groupModifiers")]
        [JsonPropertyName("groupModifiers")]
        public IEnumerable<GroupModifier>? GroupModifiers { get; set; }
        [JsonProperty("imageLinks")]
        [JsonPropertyName("imageLinks")]
        public IEnumerable<string>? ImageLinks { get; set; }
        [JsonProperty("doNotPrintInCheque")]
        [JsonPropertyName("doNotPrintInCheque")]
        public bool DoNotPrintInCheque { get; set; }
        [JsonProperty("parentGroup")]
        [JsonPropertyName("parentGroup")]
        public Guid? ParentGroup { get; set; }
        [JsonProperty("fullNameEnglish")]
        [JsonPropertyName("fullNameEnglish")]
        public string? FullNameEnglish { get; set; }
        [JsonRequired]
        [JsonProperty("useBalanceForSell")]
        [JsonPropertyName("useBalanceForSell")]
        public bool UseBalanceForSell { get; set; }
        [JsonRequired]
        [JsonProperty("canSetOpenPrice")]
        [JsonPropertyName("canSetOpenPrice")]
        public bool CanSetOpenPrice { get; set; }
        [JsonRequired]
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonProperty("code")]
        [JsonPropertyName("code")]
        public string? Code { get; set; }
        [JsonRequired]
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
        [JsonIgnore]
        public string RemoveButtonId { get; set; }
        [JsonIgnore]
        public string AddButtonId { get; set; }
        [JsonIgnore]
        public string RemoveButtonTextId { get; set; }
        [JsonIgnore]
        public string AddButtonTextId { get; set; }
        [JsonIgnore]
        public string GeneralTextOfMenuButton { get; set; } = "Выбрать";
        [JsonIgnore]
        public string AddingTextOfMenuButton { get; set; } = "+";
        [JsonIgnore]
        public string? RemovingTextOfMenuButton { get; set; }
        [JsonIgnore]
        public int TotalAmount { get; set; } = 0;

        public string ImageLink() => ImageLinks != null && ImageLinks.Any() ? ImageLinks.Last() : string.Empty;
        public bool HaveModifiers() => (Modifiers is not null && Modifiers.Any()) || (GroupModifiers is not null && GroupModifiers.Any());
        public string GetRemovingTextOfMenuButton() => 
            string.IsNullOrEmpty(RemovingTextOfMenuButton) ? HaveModifiers() ? RemovingTextOfMenuButton = "x" : RemovingTextOfMenuButton = "-" : RemovingTextOfMenuButton;
        public double? Price() => SizePrices?.FirstOrDefault()?.Price?.CurrentPrice;
        public bool HaveItems() => TotalAmount > 0;
        public string TextOfMainButtonInMenu() => HaveItems() ? AddingTextOfMenuButton : GeneralTextOfMenuButton;
        public void IncrementAmount() => TotalAmount++;
        public void DecrementAmount() => TotalAmount = TotalAmount != 0 ? --TotalAmount : TotalAmount;
    }
}