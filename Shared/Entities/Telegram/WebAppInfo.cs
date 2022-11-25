using ApiServerForTelegram.Entities.EExceptions;
using ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using WebAppAssembly.Shared.Entities.WebApp;
using WebAppAssembly.Shared.Models.Order;
using WebAppAssembly.Shared.Models.Order.Service;

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
        [JsonProperty("currentOfRub")]
        [JsonPropertyName("currentOfRub")]
        public float CurrentOfRub { get; set; }
        [JsonProperty("tlgWebAppBtnTxts")]
        [JsonPropertyName("tlgWebAppBtnTxts")]
        public TlgWebAppBtnTxts? TlgWebAppBtnTxts { get; set; }
        [JsonProperty("tlgWebAppPopupMessages")]
        [JsonPropertyName("tlgWebAppPopupMessages")]
        public TlgWebAppPopupMessages? TlgWebAppPopupMessages { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public TlgWebAppPopupMessages GetTlgWebAppPopupMessages()
            => TlgWebAppPopupMessages ?? throw new InfoException(typeof(WebAppInfo).FullName!, nameof(GetTlgWebAppPopupMessages),
                nameof(Exception), typeof(TlgWebAppPopupMessages).FullName!, ExceptionType.Null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public TransportItemDto? ProductByIdOrDefault(Guid productId, Guid? groupId = null)
            => groupId is null ? ProductByIdOrDefault(productId) : ItemCategories?.FirstOrDefault(x => x.Id == groupId)?.Items?.FirstOrDefault(x => x.ItemId == productId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public TransportItemDto? ProductByIdOrDefault(Guid productId)
            => TransportItemDtos?.FirstOrDefault(x => x.ItemId == productId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public TransportItemDto ProductById(Guid productId, Guid? groupId = null)
        {
            if (groupId is null)
                return ProductById(productId);
            var itemCategories = ItemCategories ?? throw new InfoException(typeof(WebAppInfo).FullName!, nameof(ProductById),
                nameof(Exception), $"{nameof(Enumerable)}<{typeof(TransportMenuCategoryDto).FullName!}>", ExceptionType.Null);
            var itemCategory = itemCategories.FirstOrDefault(x => x.Id == groupId) ?? throw new InfoException(typeof(WebAppInfo).FullName!,
                nameof(ProductById), nameof(Exception), $"No found the menu category by GroupId - '{groupId}'");
            var itemProducts = itemCategory.Items ?? throw new InfoException(typeof(WebAppInfo).FullName!,
                nameof(ProductById), nameof(Exception), $"{nameof(Enumerable)}<{typeof(TransportItemDto).FullName!}>", ExceptionType.Null);
            return itemProducts.FirstOrDefault(x => x.ItemId == productId) ?? throw new InfoException(typeof(WebAppInfo).FullName!,
                nameof(ProductById), nameof(Exception), $"No found the product item by ProductId - '{productId}'");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public TransportItemDto ProductById(Guid productId)
        {
            var itemProducts = TransportItemDtos ?? throw new InfoException(typeof(WebAppInfo).FullName!,
                nameof(ProductById), nameof(Exception), $"{nameof(Enumerable)}<{typeof(TransportItemDto).FullName!}>", ExceptionType.Null);
            return itemProducts.FirstOrDefault(x => x.ItemId == productId) ?? throw new InfoException(typeof(WebAppInfo).FullName!,
                nameof(ProductById), nameof(Exception), $"No found the product item by ProductId - '{productId}'");
        }
    }
}