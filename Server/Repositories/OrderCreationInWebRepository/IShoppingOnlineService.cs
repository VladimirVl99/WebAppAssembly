using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.Models.Order;

namespace WebAppAssembly.Server.Repositories.OrderCreationInWebRepository
{
    /// <summary>
    /// For working with an online store
    /// </summary>
    public interface IShoppingOnlineService
    {
        /// <summary>
        /// This class stores the necessary information for the operation of an online store.
        /// It stores information about prouducts, product categories, dilivery methods, points of sale,
        /// loyalty program and etc.
        /// </summary>
        GeneralInfoOfOnlineStore GeneralInfoOfOnlineStore { get; }
        /// <summary>
        /// The web application operation mode (test or release mode)
        /// </summary>
        bool IsReleaseMode { get; }


        /// <summary>
        /// Retrieves a customer's personal data of the order.
        /// For example: selected products, a selected delivery method, an address and etc.
        /// </summary>
        /// <param name="chatInfo"></param>
        /// <returns></returns>
        Task<PersonalInfoOfOrderByServerSide> GetPersonalDataOfOrderAsync(ChatInfo chatInfo);

        /// <summary>
        /// Saves the changed personal data in API server
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task SavePersonalDataOfOrderInServerAsync(PersonalInfoOfOrderByServerSide order);

        /// <summary>
        /// Gets an invoice link to pay the order in the Telegram interface
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<InvoiceLinkStatus> RetrieveInvoiceLinkAsync(PersonalInfoOfOrderByServerSide order);

        /// <summary>
        /// Caluculates the checkin for the order (loaylty program)
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<LoyaltyCheckinInfo> CalculateCheckinAsync(PersonalInfoOfOrderByServerSide order);

        /// <summary>
        /// Receives the wallet balance of a customer by the chat info
        /// </summary>
        /// <param name="chatInfo"></param>
        /// <returns></returns>
        Task<WalletBalance> GetCustomerWalletBalanceAsync(ChatInfo chatInfo);
    }
}
