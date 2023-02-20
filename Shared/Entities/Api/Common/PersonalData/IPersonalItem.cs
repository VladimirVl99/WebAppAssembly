using WebAppAssembly.Shared.Entities.Api.Common.Delivery;
using WebAppAssembly.Shared.Entities.Api.Common.Delivery.Addresses;
using WebAppAssembly.Shared.Entities.Api.Common.General.Terminals;

namespace WebAppAssembly.Shared.Entities.Api.Common.PersonalData
{
    /// <summary>
    /// Contains information about delivery data.
    /// </summary>
    public interface IPersonalItem
    {
        /// <summary>
        /// The delivery method.
        /// </summary>
        DeliveryServiceType DeliveryServiceType { get; set; }

        /// <summary>
        /// A pickup terminal ID.
        /// </summary>
        Guid? TerminalId { get; }

        /// <summary>
        /// Additional information about a pickup terminal ID.
        /// </summary>
        DeliveryTerminal? DeliveryTerminal { get; }

        /// <summary>
        /// A client's address.
        /// </summary>
        Address? Address { get; }
    }
}
