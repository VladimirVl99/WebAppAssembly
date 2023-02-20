using WebAppAssembly.Shared.Entities.Api.Common.Delivery;
using WebAppAssembly.Shared.Entities.Api.Common.Delivery.Addresses;
using WebAppAssembly.Shared.Entities.Api.Common.General.Terminals;
using WebAppAssembly.Shared.Entities.Api.Common.PersonalData;

namespace WebAppAssembly.Shared.Entities.OnlineStore.Personals
{
    /// <summary>
    /// Personal delivery data for orders.
    /// </summary>
    public class PersonalItem : IPersonalItem, IPersonalItemHelper
    {
        #region Properties

        public DeliveryServiceType DeliveryServiceType { get; set; }

        public Guid? TerminalId { get; private set; }

        public DeliveryTerminal? DeliveryTerminal { get; private set; }

        public Address Address { get; }

        #endregion

        #region Constructors

        public PersonalItem()
        {
            DeliveryServiceType = DeliveryServiceType.DeliveryByCourier;
            Address = new Address();
        }

        public PersonalItem(DeliveryServiceType deliveryServiceType, Guid? terminalId,
            DeliveryTerminal? deliveryTerminal, Address? address)
        {
            DeliveryServiceType = deliveryServiceType;
            TerminalId = terminalId;
            DeliveryTerminal = deliveryTerminal;
            Address = address ?? new Address();
        }

        #endregion

        #region Methods


        public void SetDeliveryMethod(DeliveryServiceType type)
            => DeliveryServiceType = type;

        public void SetDeliveryTerminal(Guid id, string name)
        {
            TerminalId = id;
            DeliveryTerminal = new DeliveryTerminal()
            {
                Id = id,
                Name = name
            };
        }

        #endregion
    }
}
