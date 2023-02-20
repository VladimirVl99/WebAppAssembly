using WebAppAssembly.Shared.Entities.Api.Common.PersonalData;
using WebAppAssembly.Shared.Entities.OnlineStore.Orders;

namespace WebAppAssembly.Shared.Entities.OnlineStore.Personals
{
    /// <summary>
    /// Information about an order, a personal data of delivery and Telegram's data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPersonalInfo<T>
        where T : Order, new()
    {
        /// <summary>
        /// Order information.
        /// </summary>
        T Order { get; }

        /// <summary>
        /// Telegram's chat information.
        /// </summary>
        TgChat ChatInfo { get; }

        /// <summary>
        /// User's personal data that is stored on the server and can be reused.
        /// </summary>
        PersonalItem PersonalItem { get; }
    }
}