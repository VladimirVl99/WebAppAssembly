using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppAssembly.Shared.Entities.Telegram;

namespace WebAppAssembly.Shared.Models.Order.Service
{
    public class OrderService
    {
        public OrderService(OrderModel orderInfo, WebAppInfo deliveryGeneralInfo)
        {
            OrderInfo = orderInfo;
            DeliveryGeneralInfo = deliveryGeneralInfo;
        }

        public OrderModel OrderInfo { get; set; }
        public WebAppInfo DeliveryGeneralInfo { get; set; }
        public bool IsDiscountBalanceConfirmed { get; set; }
        public CurrentProduct? CurrentProduct { get; set; }
        public Guid? CurrentGroupId { get; set; }
    }
}
