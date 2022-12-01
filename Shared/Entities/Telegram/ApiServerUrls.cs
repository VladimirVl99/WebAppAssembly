using Microsoft.Extensions.Configuration;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class ApiServerUrls
    {
        public ApiServerUrls(IConfiguration configuration)
        {
            SendOrder = configuration["TelegramBotProperties:sendOrderUrl"];
            Menu = configuration["TelegramBotProperties:menu"];
            CustomerBalance = configuration["TelegramBotProperties:customerBalance"];
            WebAppMenu = configuration["TelegramBotProperties:webAppMenu"];
            OrderModel = configuration["TelegramBotProperties:orderModel"];
            SaveOrderModel = configuration["TelegramBotProperties:saveOrderModel"];
            CheckOrderInfoAndCreateInvoiceLink = configuration["TelegramBotProperties:checkOrderInfoAndCreateInvoiceLink"];
            WebAppDeliveryTerminals = configuration["TelegramBotProperties:webAppDeliveryTerminals"];
            WebAppInfo = configuration["TelegramBotProperties:webAppInfo"];
            Checkin = configuration["TelegramBotProperties:checkin"];
            WalletBalance = configuration["TelegramBotProperties:walletBalance"];
            TlgMainBtnColor = configuration["Settings:TlgMainButtonColor"];
        }

        public string SendOrder { get; }
        public string Menu { get; }
        public string CustomerBalance { get; }
        public string WebAppMenu { get; }
        public string OrderModel { get; }
        public string SaveOrderModel { get; }
        public string CheckOrderInfoAndCreateInvoiceLink { get; }
        public string WebAppDeliveryTerminals { get; }
        public string WebAppInfo { get; }
        public string Checkin { get; }
        public string WalletBalance { get; }
        public string TlgMainBtnColor { get; }
    }
}
