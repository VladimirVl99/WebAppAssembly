using ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TlgWebAppNet;
using WebAppAssembly.Shared.Entities.CreateDelivery;
using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.Entities.WebApp;
using WebAppAssembly.Shared.Models.Order;

namespace WebAppAssembly.Client.Service
{
    public interface IOrderService
    {
        public OrderModel OrderInfo { get; set; }
        public WebAppInfo DeliveryGeneralInfo { get; set; }
        public bool IsDiscountBalanceConfirmed { get; set; }
        public CurrentProduct? CurrentProduct { get; set; }
        public Item? CurrItem { get; set; }
        public TransportItemDto? CurrProductItem { get; set; }
        public Guid? CurrentGroupId { get; set; }
        public bool IsReleaseMode { get; set; }
        public string TlgMainBtnColor { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ProductInfo> AddProductItemInSelectingProductPageAsync(Guid productId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ProductInfo?> RemoveProductItemInSelectingProductPageAsync(Guid productId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<ProductInfo?> RemoveProductItemInSelectingProductPageAsync(TransportItemDto product);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Task<ProductInfo> AddProductItemInShoppingCartPageAsync(ITwaNet twaNet, Guid productId,
            Guid? positionId = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Task<ProductInfo> AddProductItemInShoppingCartPageAsync(Guid productId,
            Guid? positionId = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Task<ProductInfo?> RemoveProductItemInShoppingCartPageAsync(ITwaNet twaNet, Guid productId,
            Guid? positionId = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Task<ProductInfo?> RemoveProductItemInShoppingCartPageAsync(Guid productId,
            Guid? positionId = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<bool> RemoveProductItemWithModifiersInSelectingProductPageAsync(Guid productId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<bool> RemoveProductItemWithModifiersInSelectingProductPageAsync(TransportItemDto product);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Task<ProductInfo> AddProductItemInChangingSelectedProductsWithModifiersPageAsync(Guid productId,
            Guid positionId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Task<ProductInfo?> RemoveProductItemInChangingSelectedProductsWithModifiersPageAsync(Guid productId,
            Guid positionId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="modifierId"></param>
        /// <param name="positionId"></param>
        /// <param name="modifierGroupId"></param>
        /// <returns></returns>
        Item AddModifierInSelectingModifiersAndAmountsForProductPageAsync(Guid modifierId, Guid? modifierGroupId = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="modifierId"></param>
        /// <param name="positionId"></param>
        /// <param name="modifierGroupId"></param>
        /// <returns></returns>
        Item RemoveModifierInSelectingModifiersAndAmountsForProductPageAsync(Guid modifierId, Guid? modifierGroupId = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Item AddProductInSelectingModifiersAndAmountsForProductPageAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Item? RemoveProductInSelectingModifiersAndAmountsForProductPageAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        ProductInfo AddProductWithoutModifiersForSelectingAmountsForProductsPageAsync(Guid productId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        ProductInfo AddProductWithoutModifiersInSelectingAmountsForProductsPageAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        ProductInfo? RemoveProductWithoutModifiersInSelectingAmountsForProductsPageAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="item"></param>
        void DecreaseAmountOfProduct(TransportItemDto product, Item item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        bool RemoveOrDecreaseAmountOfProduct(TransportItemDto product, Item item);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task SendChangedOrderModelToServerAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool HaveSelectedProductsAtFirst();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task CalculateLoayltyProgramAndAllowedSumAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        Task CalculateLoayltyProgramAndAllowedSumAsync(ITwaNet twaNet);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        Task<bool> CalculateLoyaltyProgramAndAllowedSumAndCheckAvailableMinSumForPayAsync(ITwaNet twaNet);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        Task<bool> CheckAvailableMinSumForPayAsync(ITwaNet twaNet);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        double FinalSumOfOrder();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<WalletBalance> RetrieveWalletBalanceAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool CheckSelectingModifiersInSelectingModifiersAndAmountsForProductPageAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        Task<bool> IsNecessaryDataOfOrderCorrect(ITwaNet twaNet);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<bool> IsWalletBalanceChangedInIikoCardAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<InvoiceLinkStatus> CreateInvoiceUrlLinkAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        Task<bool> CreateInvoiceLinkAsync(ITwaNet twaNet);

        /// <summary>
        /// 
        /// </summary>
        void CancelCurrSelectedItemWithModifiers();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task CancelCurrSimilarSelectedItemsWithModifiersAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        CurrentProduct GetCurrProduct();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task SetPickupTerminalAsync(ITwaNet twaNet, Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task SetPickupTerminalAsync(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task RemoveAllSelectedProductsInShoppingCartPageAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string InfoAboutCreatedOrderForTest();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizeId"></param>
        /// <returns></returns>
        Item ChangeProductSize(Guid sizeId);
    }
}
