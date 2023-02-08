using WebAppAssembly.Shared.Entities.Api.Common.Delivery.Addresses;
using WebAppAssembly.Shared.Entities.Api.Common.PersonalData;
using WebAppAssembly.Shared.Entities.OnlineStore.Orders;

namespace WebAppAssembly.Shared.Models.OrderData
{
    /// <summary>
    /// 
    /// </summary>
    public class PersonalInfo<T> : IPersonalInfo<T>
        where T : Order, new()
    {
        #region Properties

        public T Order { get; private set; }

        public TgChat ChatInfo { get; private set; }

        public PersonalItem PersonalItem { get; private set; }

        #endregion


        #region Constructors

        public PersonalInfo()
        {
            Order = new T();
            ChatInfo = new TgChat();
            PersonalItem = new PersonalItem()
            {
                Address = new Address()
            };
        }

        public PersonalInfo(T order)
            : this()
        {
            Order = order;
        }

        public PersonalInfo(T order, TgChat chat, PersonalItem personalItem)
        {
            Order = order;
            ChatInfo = chat;
            PersonalItem = personalItem;
            PersonalItem.Address = new Address();
        }

        #endregion
    }
}