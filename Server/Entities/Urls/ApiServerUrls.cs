namespace WebAppAssembly.Server.Entities.Urls
{
    /// <summary>
    /// Urls to the main API server.
    /// </summary>
    public class ApiServerUrls
    {
        /// <summary>
        /// For retrieving the personal data of an order.
        /// </summary>
        public string PersonalInfo { get; }

        /// <summary>
        /// For saving the personal data of an order.
        /// </summary>
        public string SavePersonalInfo { get; }

        /// <summary>
        /// For creating an invoice link by the personal order.
        /// </summary>
        public string CheckPersonalOrderAndCreateInvoiceLink { get; }

        /// <summary>
        /// For retrieving common data for the online store.
        /// </summary>
        public string CommonDataForOnlineStore { get; }

        /// <summary>
        /// For calculating an order's checkin.
        /// </summary>
        public string Checkin { get; }

        /// <summary>
        /// For retrieving a customer's wallet balance.
        /// </summary>
        public string WalletBalance { get; }


        public ApiServerUrls(IConfiguration configuration)
        {
            PersonalInfo = configuration["TelegramBotProperties:orderModel"];
            SavePersonalInfo = configuration["TelegramBotProperties:saveOrderModel"];
            CheckPersonalOrderAndCreateInvoiceLink = configuration["TelegramBotProperties:checkOrderInfoAndCreateInvoiceLink"];
            CommonDataForOnlineStore = configuration["TelegramBotProperties:webAppInfo"];
            Checkin = configuration["TelegramBotProperties:checkin"];
            WalletBalance = configuration["TelegramBotProperties:walletBalance"];
        }
    }
}
