using Newtonsoft.Json;
using WebAppAssembly.Shared.Entities.Api.Common.HttpInfos;
using WebAppAssembly.Shared.Entities.Api.Common.Loylties;
using SourceLoyaltyCheckinInfo = WebAppAssembly.Shared.Entities.Api.Common.Loylties.LoyaltyCheckinInfo;

namespace WebAppAssembly.Shared.Entities.Api.Requests.Loyalties
{
    /// <summary>
    /// Information about the loyalty checkin calculation.
    /// </summary>
    [JsonObject]
    public class LoyaltyCheckinInfo : SourceLoyaltyCheckinInfo
    {
        public LoyaltyCheckinInfo(bool ok, Checkin? checkin = null, HttpResponseShortInfo? httpResponseInfo = null)
        {
            Checkin = checkin;
            Ok = ok;
            HttpResponseInfo = httpResponseInfo;
        }
    }
}
