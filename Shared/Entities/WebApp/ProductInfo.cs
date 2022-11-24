using ApiServerForTelegram.Entities.EExceptions;
using ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WebAppAssembly.Shared.Entities.CreateDelivery;
using WebAppAssembly.Shared.Models.Order.Service;

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

        public TransportItemDto GeneralProductInfo { get; set; }
        public Item Item { get; set; }
        public bool IsFirstSelected { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public double TotalSumOfSelectedProductWithModifiers()
        {
            var productPrice = GeneralProductInfo.Price(Item.ProductSizeId) ?? throw new InfoException(typeof(OrderService).FullName!,
                nameof(TotalSumOfSelectedProductWithModifiers), nameof(Exception), $"Price of product by ID - '{GeneralProductInfo.ItemId}' can't be null");
            return (productPrice + PriceWithModifiersByProductId()) * Item.Amount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public double PriceWithModifiersByProductId()
        {
            var modifiers = Item.SelectedModifiers();
            double total = 0;
            foreach (var modifier in modifiers)
                total += (modifier.Price ?? throw new InfoException(typeof(OrderService).FullName!,
                    nameof(TotalSumOfSelectedProductWithModifiers), nameof(Exception), $"Price of modifier by ID - '{modifier.ProductId}' can't be null")) * modifier.Amount;
            return total;
        }
    }
}
