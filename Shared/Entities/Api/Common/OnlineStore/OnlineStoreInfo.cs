using Newtonsoft.Json;
using WebAppAssembly.Shared.Entities.Api.Common.PersonalData;

namespace WebAppAssembly.Shared.Entities.Api.Common.OnlineStore
{
    /// <summary>
    /// Contains menus obtained via iikoTransport, customer's personal data about delivery,
    /// selected products of the order and settings for the web application.
    /// </summary>
    [JsonObject]
    public class OnlineStoreInfo
    {
        #region Properties

        /// <summary>
        /// Customer's personal data of the order.
        /// </summary>
        [JsonProperty(PropertyName = "personalOrder", Required = Required.Always)]
        public PersonalInfo PersonalOrderInfo { get; set; } = default!;

        /// <summary>
        /// Menus, loyalty programs, delivery data via iikoTransport.
        /// Also settings for the web application.
        /// </summary>
        [JsonProperty(PropertyName = "onlineStoreItem", Required = Required.Always)]
        public OnlineStoreItem OnlineStoreItem { get; set; } = default!;

        /// <summary>
        /// Web application mode.
        /// </summary>
        [JsonProperty(PropertyName = "isReleaseMode", Required = Required.Always)]
        public bool IsReleaseMode { get; set; }

        #endregion

        #region Constructors

        public OnlineStoreInfo() { }

        public OnlineStoreInfo(OnlineStoreItem onlineStoreItem, bool isReleaseMode)
        {
            OnlineStoreItem = onlineStoreItem;
            IsReleaseMode = isReleaseMode;
        }

        #endregion
    }
}
