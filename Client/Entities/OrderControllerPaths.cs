using OrderControllerPathsOfServer = WebAppAssembly.Shared.Entities.OfServerSide.OrderControllerPaths;

namespace WebAppAssembly.Client.Entities
{
    public static class OrderControllerPaths
    {
        private static readonly string PathOfController = "Order/";
        public static string RetrieveMainInfoForWebAppOrder { get; } = PathOfController + OrderControllerPathsOfServer.MainInfoForWebAppOrder;
    }
}
