using WebAppAssembly.Shared.Entities.Api.Common.PersonalData;
using WebAppAssembly.Shared.Entities.OnlineStore.Orders;

namespace WebAppAssembly.Shared.Models.OrderData
{
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