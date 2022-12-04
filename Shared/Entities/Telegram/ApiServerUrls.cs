using Microsoft.Extensions.Configuration;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class ApiServerUrls
    {
        public ApiServerUrls(IConfiguration configuration)
        {
            OrderModel = configuration["TelegramBotProperties:orderModel"];
            SaveOrderModel = configuration["TelegramBotProperties:saveOrderModel"];
            CheckOrderInfoAndCreateInvoiceLink = configuration["TelegramBotProperties:checkOrderInfoAndCreateInvoiceLink"];
            WebAppInfo = configuration["TelegramBotProperties:webAppInfo"];
            Checkin = configuration["TelegramBotProperties:checkin"];
            WalletBalance = configuration["TelegramBotProperties:walletBalance"];
            TlgMainBtnColor = configuration["Settings:TlgMainButtonColor"];
        }

        public string OrderModel { get; }
        public string SaveOrderModel { get; }
        public string CheckOrderInfoAndCreateInvoiceLink { get; }
        public string WebAppInfo { get; }
        public string Checkin { get; }
        public string WalletBalance { get; }
        public string TlgMainBtnColor { get; }
    }
}
