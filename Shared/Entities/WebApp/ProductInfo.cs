using ApiServerForTelegram.Entities.EExceptions;
using ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WebAppAssembly.Shared.Entities.CreateDelivery;
using WebAppAssembly.Shared.Entities.EMenu;

namespace WebAppAssembly.Shared.Entities.WebApp
{
    public class ProductInfo
    {
        public ProductInfo(TransportItemDto generalProductInfo, Item item, bool isFirstSelected)
        {
            GeneralProductInfo = generalProductInfo;
            Item = item;
            IsFirstSelected = isFirstSelected;
        }

        public ProductInfo(TransportItemDto generalProductInfo, Item item)
        {
            GeneralProductInfo = generalProductInfo;
            Item = item;
            IsFirstSelected = false;
        }

        public TransportItemDto GeneralProductInfo { get; set; }
        public Item Item { get; set; }
        public bool IsFirstSelected { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public double TotalSumOfSelectedProductWithModifiers() => Item.TotalPrice ?? throw new InfoException(typeof(ProductInfo).FullName!,
            nameof(TotalSumOfSelectedProductWithModifiers), nameof(Exception), $"Total price of modifier by ID - '{Item.ProductId}' can't be null");
    }
}
