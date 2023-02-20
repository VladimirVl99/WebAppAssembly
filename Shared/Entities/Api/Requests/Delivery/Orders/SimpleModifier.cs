using Newtonsoft.Json;
using SourceSimpleModifier = WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders.SimpleModifier;

namespace WebAppAssembly.Shared.Entities.Api.Requests.Delivery.Orders
{
    /// <summary>
    /// Information about an item's modifier.
    /// </summary>
    [JsonObject]
    public class SimpleModifier : SourceSimpleModifier
    {
        public SimpleModifier(Guid id, string name, double minAmount, double maxAmount)
        {
            Id = id;
            MinQuantity = minAmount;
            MaxQuantity = maxAmount;
            Name = name;
        }
    }
}
