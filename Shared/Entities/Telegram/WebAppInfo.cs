using WebAppAssembly.Shared.Models.Order;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class WebAppInfo
    {
        public WebAppMenu? WebAppMenu { get; set; }
        public IEnumerable<DeliveryTerminal>? DeliveryTerminals { get; set; }
        public PickupType PickupType { get; set; }
        public bool UseIikoBizProgram { get; set; }
        public bool UseCoupon { get; set; }
        public bool UseDiscountBalance { get; set; }
    }
}