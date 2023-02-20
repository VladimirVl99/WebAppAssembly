using Newtonsoft.Json;
using SourceSimpleGroupModifier = WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders.SimpleGroupModifier;

namespace WebAppAssembly.Shared.Entities.Api.Requests.Delivery.Orders
{
    /// <summary>
    /// Information about an item's modifier group.
    /// </summary>
    [JsonObject]
    public class SimpleGroupModifier : SourceSimpleGroupModifier
    {
        public SimpleGroupModifier(Guid id, string name, double minAmount, double maxAmount)
        {
            Id = id;
            MinQuantity = minAmount;
            MaxQuantity = maxAmount;
            Name = name;
        }
    }
}
