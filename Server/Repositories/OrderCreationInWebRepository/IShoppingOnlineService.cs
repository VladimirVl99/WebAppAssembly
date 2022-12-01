using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.Models.Order;

namespace WebAppAssembly.Server.Repositories.OrderCreationInWebRepository
{
    public interface IShoppingOnlineService
    {
        DeliveryGeneralInfo DeliveryGeneralInfo { get; }
        bool IsReleaseMode { get; }


        Task<OrderModelOfServer> GetOrderModelCashAsync(long chatId);
        Task SendOrderInfoToServerAsync(OrderModelOfServer order);
        Task<InvoiceLinkStatus> CreateInvoiceLinkAsync(OrderModelOfServer order);
        Task<LoyaltyCheckinInfo> CalculateCheckinAsync(OrderModelOfServer order);
        Task<WalletBalance> GetCustomerWalletBalanceAsync(ChatInfo chatInfo);
    }
}
