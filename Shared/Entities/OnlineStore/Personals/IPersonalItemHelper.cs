using WebAppAssembly.Shared.Entities.Api.Common.Delivery;

namespace WebAppAssembly.Shared.Entities.OnlineStore.Personals
{
    /// <summary>
    /// Auxiliary methods for a person item.
    /// </summary>
    internal interface IPersonalItemHelper
    {
        /// <summary>
        /// Sets/changes the delivery method.
        /// </summary>
        /// <param name="type"></param>
        void SetDeliveryMethod(DeliveryServiceType type);

        /// <summary>
        /// Sets/changes the delivery terminal.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        void SetDeliveryTerminal(Guid id, string name);
    }
}
