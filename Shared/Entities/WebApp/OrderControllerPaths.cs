using OrderControllerPathsOfServer = WebAppAssembly.Shared.Entities.OfServerSide.OrderControllerPaths;

namespace WebAppAssembly.Shared.Entities.WebApp
{
    public static class OrderControllerPaths
    {
        private static readonly string PathOfController = "Order/";
        public static string RetrieveMainInfoForWebAppOrder { get; } = PathOfController + OrderControllerPathsOfServer.MainInfoForWebAppOrder;
        public static string SendChangedOrderModelToServer { get; } = PathOfController + OrderControllerPathsOfServer.SendChangedOrderModelToServer;
        public static string CalculateCheckin { get; } = PathOfController + OrderControllerPathsOfServer.CalculateCheckin;
        public static string RetreiveWalletBalance { get; } = PathOfController + OrderControllerPathsOfServer.RetreiveWalletBalance;
    }
}
