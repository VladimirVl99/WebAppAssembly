namespace WebAppAssembly.Shared.Entities.OfServerSide
{
    public static class OrderControllerPaths
    {
        public static string MainInfoForWebAppOrder { get; } = "mainInfoForWebAppOrder";
        public static string SendChangedOrderModelToServer { get; } = "saveOrderInfoInServer";
        public static string CalculateCheckin { get; } = "calculateCheckin";
        public static string RetreiveWalletBalance { get; } = "walletBalance";
    }
}
