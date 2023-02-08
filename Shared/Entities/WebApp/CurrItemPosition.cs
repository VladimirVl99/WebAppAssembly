using ApiServerForTelegram.Entities.EExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.RetrieveExternalMenuByID;
using WebAppAssembly.Shared.Entities.OnlineStore.Orders;
using OrderItem = WebAppAssembly.Shared.Entities.OnlineStore.Orders.OrderItem;

namespace WebAppAssembly.Shared.Entities.WebApp
{
    public class CurrItemPosition
    {
        public CurrItemPosition(TransportItemDto productInfo, IOrderItemProcessing? item = null)
        {
            ProductInfo = productInfo;
            Item = item;
        }

        public TransportItemDto ProductInfo { get; set; }
        public IOrderItemProcessing? Item { get; set; }

        public IOrderItemProcessing GetItem() => Item ?? throw new InfoException(typeof(CurrItemPosition).FullName!,
            nameof(GetItem), nameof(Exception), typeof(OrderItem).FullName!, ExceptionType.Null);
    }
}
