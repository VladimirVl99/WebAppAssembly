using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus
{
    /// <summary>
    /// Contains information about an external product.
    /// </summary>
    [JsonObject]
    public class Product
    {
        /// <summary>
        /// Item sizes.
        /// </summary>
        [JsonProperty(PropertyName = "itemSizes", Required = Required.Always)]
        public IEnumerable<Size> Sizes { get; set; } = default!;

        /// <summary>
        /// Product code.
        /// </summary>
        [JsonProperty(PropertyName = "sku", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Code { get; set; }

        /// <summary>
        /// Product name.
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Product description.
        /// </summary>
        [JsonProperty(PropertyName = "description", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Description { get; set; }

        /// <summary>
        /// Allergen groups of the product.
        /// </summary>
        [JsonProperty(PropertyName = "allergenGroups", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<AllergenGroup>? AllergenGroups { get; set; }

        /// <summary>
        /// Product ID.
        /// </summary>
        [JsonProperty(PropertyName = "itemId", Required = Required.Always)]
        public Guid Id { get; set; }

        /// <summary>
        /// Modifier schema ID.
        /// </summary>
        [JsonProperty(PropertyName = "modifierSchemaId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? ModifierSchemaId { get; set; }

        /// <summary>
        /// Modifier schema name.
        /// </summary>
        [JsonProperty(PropertyName = "modifierSchemaName", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ModifierSchemaName { get; set; }

        /// <summary>
        /// Tax category.
        /// </summary>
        [JsonProperty(PropertyName = "taxCategory", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<TaxCategory>? TaxCategories { get; set; }

        /// <summary>
        /// Tags.
        /// </summary>
        [JsonProperty(PropertyName = "tags", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<InternalTag>? InternalTags { get; set; }

        /// <summary>
        /// Labels.
        /// </summary>
        [JsonProperty(PropertyName = "labeles", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<ExternalTag>? ExternalTags { get; set; }

        /// <summary>
        /// Enum: "Product" "Compound".
        /// Product or compound. Depends on modifiers scheme existence.
        /// </summary>
        [JsonProperty(PropertyName = "orderItemType", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderItemType OrderItemType { get; set; }
    }
}
