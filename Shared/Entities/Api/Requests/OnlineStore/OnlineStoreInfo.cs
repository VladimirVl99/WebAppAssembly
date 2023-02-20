using Newtonsoft.Json;
using WebAppAssembly.Shared.Entities.Api.Common.OnlineStore;
using SourceOnlineStoreInfo = WebAppAssembly.Shared.Entities.Api.Common.OnlineStore.OnlineStoreInfo;

namespace WebAppAssembly.Shared.Entities.Api.Requests.OnlineStore
{
    /// <summary>
    /// Contains menus obtained via iikoTransport, customer's personal data about delivery,
    /// selected products of the order and settings for the web application.
    /// </summary>
    [JsonObject]
    public class OnlineStoreInfo : SourceOnlineStoreInfo
    {
        public OnlineStoreInfo(OnlineStoreItem onlineStoreItem, bool isReleaseMode)
        {
            OnlineStoreItem = onlineStoreItem;
            IsReleaseMode = isReleaseMode;
        }
    }
}
