namespace WebAppAssembly.Shared.Models.Order
{
    // Flags for razor pages which is using Telegram app
    public class RazorOrderInfo
    {
        public PageType PageType { get; set; }
        public OrderStatusType OrderStatusType { get; set; }
        public bool IsTelegramConnectionInit { get; set; } = false;
    }
}