namespace WebAppAssembly.Shared.Entities.Urls
{
    /// <summary>
    /// Urls to a web application Api server for orders.
    /// </summary>
    public static class OrderControllerUrls
    {
        /// <summary>
        /// Common url path for orders.
        /// </summary>
        private const string PathOfController = "Order/";


        /// <summary>
        /// For retrieving the main data from a server.
        /// </summary>
        public static string RetrieveMainInfoForWebAppOrder { get; } = PathOfController + "mainInfoForWebAppOrder";

        /// <summary>
        /// For sending the data from Web App to a server.
        /// </summary>
        public static string SendChangedOrderModelToServer { get; } = PathOfController + "saveOrderInfoInServer";

        /// <summary>
        /// For calculating the checkin for an order.
        /// </summary>
        public static string CalculateCheckin { get; } = PathOfController + "calculateCheckin";

        /// <summary>
        /// For retrieving a customer wallet balance.
        /// </summary>
        public static string RetreiveWalletBalance { get; } = PathOfController + "walletBalance";

        /// <summary>
        /// For creating a link for an invoice.
        /// </summary>
        public static string CreateInvoiceLink { get; } = PathOfController + "createInvoiceLink";
    }
}
