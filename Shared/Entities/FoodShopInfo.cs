using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.Models.Order;

namespace WebAppAssembly.Shared.Entities
{
    public class FoodShopInfo
    {
        public FoodShopInfo() { }

        public FoodShopInfo(IEnumerable<DeliveryTerminal>? deliveryTerminals, WebAppMenu? webAppMenu, bool? isTestMode, WebAppInfo? webAppInfo)
        {
            DeliveryTerminals = deliveryTerminals;
            WebAppMenu = webAppMenu;
            IsTestMode = isTestMode;
            WebAppInfo = webAppInfo;
        }

        public IEnumerable<DeliveryTerminal>? DeliveryTerminals { get; set; }
        public WebAppMenu? WebAppMenu { get; set; }
        public bool? IsTestMode { get; set; }
        public WebAppInfo? WebAppInfo { get; set; }
    }
}
