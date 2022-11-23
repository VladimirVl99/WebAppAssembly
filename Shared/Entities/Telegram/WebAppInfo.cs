using ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using WebAppAssembly.Shared.Models.Order;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class WebAppInfo
    {
        [JsonProperty("itemCategories")]
        [JsonPropertyName("itemCategories")]
        public IEnumerable<TransportMenuCategoryDto>? ItemCategories { get; set; }
        [JsonProperty("productItems")]
        [JsonPropertyName("productItems")]
        public IEnumerable<TransportItemDto>? TransportItemDtos
        { 
            get
            {
                if (ItemCategories is not null)
                {
                    var items = new List<TransportItemDto>();
                    foreach (var item in ItemCategories)
                        if (item.Items is not null) items.AddRange(item.Items.ToList());
                    return items.ToArray();
                }
                return null;
            }
        }
        [JsonProperty("deliveryTerminals")]
        [JsonPropertyName("deliveryTerminals")]
        public IEnumerable<DeliveryTerminal>? DeliveryTerminals { get; set; }
        [JsonProperty("pickupType")]
        [JsonPropertyName("pickupType")]
        public PickupType PickupType { get; set; }
        [JsonProperty("useIikoBizProgram")]
        [JsonPropertyName("useIikoBizProgram")]
        public bool UseIikoBizProgram { get; set; }
        [JsonProperty("useCoupon")]
        [JsonPropertyName("useCoupon")]
        public bool UseCoupon { get; set; }
        [JsonProperty("useDiscountBalance")]
        [JsonPropertyName("useDiscountBalance")]
        public bool UseDiscountBalance { get; set; }
        [JsonProperty("CurrentOfRub")]
        [JsonPropertyName("CurrentOfRub")]
        public float CurrentOfRub { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public bool IsTestMode { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public long TestChatId { get; set; }

        public TransportItemDto? ProductById(Guid groupId, Guid productId)
        {
            if (groupId == Guid.Empty) return ProductById(productId);
            return ItemCategories?.FirstOrDefault(x => x.Id == groupId)?.Items?.FirstOrDefault(x => x.ItemId == productId);
        }

        public TransportItemDto? ProductById(Guid productId) =>
            TransportItemDtos?.FirstOrDefault(x => x.ItemId == productId);
    }
}