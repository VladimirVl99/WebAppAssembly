using Newtonsoft.Json;

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
        public double? FatAmount { get; set; }
        [JsonProperty("proteinsAmount")]
        public double? ProteinsAmount { get; set; }
        [JsonProperty("carbohydratesAmount")]
        public double? CarbohydratesAmount { get; set; }
        [JsonProperty("energyAmount")]
        public double? EnergyAmount { get; set; }
        [JsonProperty("fatFullAmount")]
        public double? FatFullAmount { get; set; }
        [JsonProperty("proteinsFullAmount")]
        public double? ProteinsFullAmount { get; set; }
        [JsonProperty("carbohydratesFullAmount")]
        public double? CarbohydratesFullAmount { get; set; }
        [JsonProperty("energyFullAmount")]
        public double? EnergyFullAmount { get; set; }
        [JsonProperty("weight")]
        public double? Weight { get; set; }
        [JsonProperty("groupId")]
        public Guid? GroupId { get; set; }
        [JsonProperty("productCategoryId")]
        public Guid? ProductCategoryId { get; set; }
        [JsonProperty("type")]
        public string? Type { get; set; }
        [JsonProperty("orderItemType")]
        public string? OrderItemType { get; set; }
        [JsonProperty("modifierSchemaId")]
        public Guid? ModifierSchemaId { get; set; }
        [JsonProperty("modifierSchemaName")]
        public string? ModifierSchemaName { get; set; }
        [JsonRequired]
        [JsonProperty("splittable")]
        public bool Splittable { get; set; }
        [JsonProperty("measureUnit")]
        public string? MeasureUnit { get; set; }
        [JsonProperty("sizePrices")]
        public IEnumerable<SizePrice>? SizePrices { get; set; }
        [JsonProperty("modifiers")]
        public IEnumerable<Modifier>? Modifiers { get; set; }
        [JsonProperty("groupModifiers")]
        public IEnumerable<GroupModifier>? GroupModifiers { get; set; }
        [JsonProperty("imageLinks")]
        public IEnumerable<string>? ImageLinks { get; set; }
        [JsonProperty("doNotPrintInCheque")]
        public bool DoNotPrintInCheque { get; set; }
        [JsonProperty("parentGroup")]
        public Guid? ParentGroup { get; set; }
        [JsonProperty("order")]
        public int Order { get; set; }
        [JsonProperty("fullNameEnglish")]
        public string? FullNameEnglish { get; set; }
        [JsonRequired]
        [JsonProperty("useBalanceForSell")]
        public bool UseBalanceForSell { get; set; }
        [JsonRequired]
        [JsonProperty("canSetOpenPrice")]
        public bool CanSetOpenPrice { get; set; }
        [JsonRequired]
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("code")]
        public string? Code { get; set; }
        [JsonRequired]
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
        [JsonIgnore]
        public bool HaveItemsBeforeRender { get; set; } = false;
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