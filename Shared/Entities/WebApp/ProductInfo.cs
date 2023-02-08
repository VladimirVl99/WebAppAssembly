using ApiServerForTelegram.Entities.EExceptions;
using ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WebAppAssembly.Shared.Entities.EMenu;
using WebAppAssembly.Shared.Entities.OnlineStore.Orders;

namespace WebAppAssembly.Shared.Entities.WebApp
{
    /// <summary>
    /// ???
    /// </summary>
    public class ProductInfo
    {
        public ProductInfo(OrderItem item, bool isFirstSelected)
        {
            Item = item;
            IsFirstSelected = isFirstSelected;
        }

        public ProductInfo(OrderItem item)
        {
            Item = item;
            IsFirstSelected = false;
        }

        public OrderItem Item { get; set; }
        public bool IsFirstSelected { get; set; }
    }
}
