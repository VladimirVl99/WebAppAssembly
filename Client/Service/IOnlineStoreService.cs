using ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID;
using Microsoft.JSInterop;
using TlgWebAppNet;
using WebAppAssembly.Shared.Entities.CreateDelivery;
using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.Entities.WebApp;
using WebAppAssembly.Shared.Models.Order;

namespace WebAppAssembly.Client.Service
{
    /// <summary>
    /// Implements the work of caluculating operations for orders in the online store on the client side.
    /// Note: There is possible to send a customer's own data of the order to the API server in the some methods.
    /// </summary>
    public interface IOnlineStoreService
    {
        /// <summary>
        /// A customer's personal data of the order.
        /// For example: selected products, a selected delivery method, an address and etc.
        /// </summary>
        PersonalInfoOfOrder PersonalInfoOfOrder { get; set; }
        /// <summary>
        /// Stores the necessary information for the operation of the online store.
        /// It stores information about prouducts, product categories, dilivery methods, points of sale,
        /// loyalty program and etc.
        /// </summary>
        GeneralInfoOfOnlineStore GeneralInfoOfOnlineStore { get; set; }
        /// <summary>
        /// A selected product.
        /// It is used on those pages where to need to work with an individual product position/positions.
        /// </summary>
        CurrentProduct? CurrentProduct { get; set; }
        /// <summary>
        /// The current product position that is currently being edited.
        /// </summary>
        Item? CurrItem { get; set; }
        /// <summary>
        /// The current product that is currently being used.
        /// </summary>
        TransportItemDto? CurrProductItem { get; set; }
        /// <summary>
        /// The current group. It's used for displaying products of the group.
        /// </summary>
        Guid? CurrentGroupId { get; set; }
        /// <summary>
        /// The web application operation mode (test or release mode).
        /// </summary>
        bool IsReleaseMode { get; set; }
        /// <summary>
        /// Color of the Telegram's main button.
        /// </summary>
        string TlgMainBtnColor { get; set; }
        /// <summary>
        /// Allows or not to work with the loyalty program.
        /// This flag is used on the basket page.
        /// </summary>
        bool IsLoyaltyProgramAvailableForProcess { get; }


        /// <summary>
        /// Receives and verifies data from the API server for the online store.
        /// Receives the necessary information for the operation of an online store.
        /// Also receives and verifies the customer's personal data of the order.
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="urlPathOfMainInfo"></param>
        /// <returns></returns>
        Task InitOnlineStoreServiceAsync(long chatId, string urlPathOfMainInfo);

        /// <summary>
        /// Adds a new item for a product that contains modifiers and sizes every time this method is called.
        /// Adds a new item for a product without modifiers and sizes then increases the quantity for this item.
        /// Also returns the bool value that the first item has been added
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ProductInfo> AddOrIncreaseItemPositionForSelectingProductPageAsync(Guid productId);

        /// <summary>
        /// Decreases the number of positions of the selected product wihtout modifiers and sizes.
        /// Removes the item if the number of positions is equal to zero.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ProductInfo?> RemoveOrDecreaseItemPositionForSelectingProductPageAsync(Guid productId);

        /// <summary>
        /// Decreases the number of positions of the selected product wihtout modifiers and sizes.
        /// Removes the item if the number of positions is equal to zero.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<ProductInfo?> RemoveOrDecreaseItemPositionForSelectingProductPageAsync(TransportItemDto product);

        /// <summary>
        /// Increases the number of positions of the selected product item.
        /// Recalculates the loaylty program with the allowed bonus sum to pay when the loyalty system is used.
        /// Supports operation only when working with the Telegram application, in other cases it is not recommended for use.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="twaNet"></param>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Task<ProductInfo> IncreaseItemPositionForShoppingCartPageAsync(IJSRuntime jsRuntime, ITwaNet twaNet, Guid productId,
            Guid? positionId = null);

        /// <summary>
        /// Increases the number of positions of the selected product item.
        /// Recalculates the loaylty program with the allowed bonus sum to pay when the loyalty system is used.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Task<ProductInfo> IncreaseItemPositionForShoppingCartPageAsync(Guid productId, Guid? positionId = null);

        /// <summary>
        /// Decrease the number of positions of the selected product item.
        /// Removes the item if the number of positions is equal to zero.
        /// Recalculates the loaylty program with the allowed bonus sum to pay when the loyalty system is used.
        /// Supports operation only when working with the Telegram application, in other cases it is not recommended for use.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="twaNet"></param>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Task<ProductInfo?> RemoveOrDecreaseItemPositionForShoppingCartPageAsync(IJSRuntime jsRuntime, ITwaNet twaNet, Guid productId,
            Guid? positionId = null);

        /// <summary>
        /// Decrease the number of positions of the selected product item.
        /// Removes the item if the number of positions is equal to zero.
        /// Recalculates the loaylty program with the allowed bonus sum to pay when the loyalty system is used.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Task<ProductInfo?> RemoveOrDecreaseItemPositionForShoppingCartPageAsync(Guid productId, Guid? positionId = null);

        /// <summary>
        /// If there are several items in the order with the same product ID (as a rule, these are products with modifiers and sizes)
        /// then just sets the current product for the order and returns the 'true'.
        /// In other case, to decrease the number of positions of the remainder product item and removes it
        /// if the number of positions is equal to zero.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<bool> RemoveOrDecreaseItemPositionWithModifiersOrSizesForSelectingProductPageAsync(Guid productId);

        /// <summary>
        /// If there are several items in the order with the same product ID (as a rule, these are products with modifiers and sizes)
        /// then just sets the current product for the order and returns the 'true'.
        /// In other case, to decrease the number of positions of the remainder product item and removes it
        /// if the number of positions is equal to zero.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<bool> RemoveOrDecreaseItemPositionWithModifiersOrSizesForSelectingProductPageAsync(TransportItemDto product);

        /// <summary>
        /// Increases the number of positions of the selected product item.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Task<ProductInfo> IncreaseItemPositionWithModifiersOrSizesForChangingSelectedProductsWithModifiersPageAsync(Guid productId,
            Guid positionId);

        /// <summary>
        /// Decreases the number of positions of the selected product item and removes it if the number of positions is equal to zero.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Task<ProductInfo?> RemoveOrDecreaseItemPositionWithModifiersOrSizesForChangingSelectedProductsWithModifiersPageAsync(Guid productId,
            Guid positionId);

        /// <summary>
        /// Increase the selected modifier's number of positions of the selected product item.
        /// </summary>
        /// <param name="modifierId"></param>
        /// <param name="modifierGroupId"></param>
        /// <returns></returns>
        Item IncreaseModifierItemPositionForSelectingModifiersAndAmountsForProductPageAsync(Guid modifierId, Guid? modifierGroupId = null);

        /// <summary>
        /// Decrease the selected modifier's number of positions of the selected product item.
        /// </summary>
        /// <param name="modifierId"></param>
        /// <param name="modifierGroupId"></param>
        /// <returns></returns>
        Item DecreaseModifierItemPositionForSelectingModifiersAndAmountsForProductPageAsync(Guid modifierId, Guid? modifierGroupId = null);

        /// <summary>
        /// Increases the number of positions of the selected product item.
        /// </summary>
        /// <returns></returns>
        Item IncreaseItemPositionForSelectingModifiersAndAmountsForProductPageAsync();

        /// <summary>
        /// Decreases the number of positions of the selected product item and removes it if the number of positions is equal to zero.
        /// </summary>
        /// <returns></returns>
        Item? RemoveOrDecreaseItemPositionForSelectingModifiersAndAmountsForProductPageAsync();

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
        void DecreaseAmountOfProduct(Item item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        bool RemoveOrDecreaseAmountOfProduct(Item item);

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
        Task CalculateLoayltyProgramAndAllowedSumAsync(IJSRuntime jsRuntime, ITwaNet twaNet);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        Task<bool> CalculateLoyaltyProgramAndAllowedSumAndCheckAvailableMinSumForPayAsync(IJSRuntime jsRuntime, ITwaNet twaNet);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        Task<bool> CheckAvailableMinSumForPayAsync(IJSRuntime jsRuntime, ITwaNet twaNet);

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
        bool CheckSelectedModifiersInSelectingModifiersAndAmountsForProductPageAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        Task<bool> IsNecessaryDataOfOrderCorrect(IJSRuntime jsRuntime, ITwaNet twaNet);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<bool> IsWalletBalanceChangedInIikoCardAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<InvoiceLinkStatus> TryToCreateInvoiceUrlLinkAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        Task<bool> TryToCreateInvoiceLinkAsync(IJSRuntime jsRuntime, ITwaNet twaNet);

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
        Task SetPickupTerminalWithLoyaltyProgramProcessAsync(IJSRuntime jsRuntime, ITwaNet twaNet, Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task SetPickupTerminalWithLoyaltyProgramProcessAsync(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        void SetPickupTerminal(Guid id);

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
