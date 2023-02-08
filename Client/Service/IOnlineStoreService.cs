using Microsoft.JSInterop;
using TlgWebAppNet;
using WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.RetrieveExternalMenuByID;
using WebAppAssembly.Shared.Entities.OnlineStore;
using WebAppAssembly.Shared.Entities.OnlineStore.Orders;
using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.Entities.WebApp;
using WebAppAssembly.Shared.Entities.WebAppPage;
using WebAppAssembly.Shared.Models.OrderData;

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
        IPersonalInfo<OrderClient> PersonalInfo { get; }
        /// <summary>
        /// Stores the necessary information for the operation of the online store.
        /// It stores information about prouducts, product categories, dilivery methods, points of sale,
        /// loyalty program and etc.
        /// </summary>
        OnlineStoreItem CommonItem { get; }
        /// <summary>
        /// The current product position that is currently being edited.
        /// </summary>
        CurrItemPosition? CurrItemPosition { get; }
        /// <summary>
        /// The current group. It's used for displaying products of the group.
        /// </summary>
        Guid? CurrentGroupId { get; }
        /// <summary>
        /// The web application operation mode (test or release mode).
        /// </summary>
        bool IsReleaseMode { get; }
        /// <summary>
        /// Color of the Telegram's main button.
        /// </summary>
        string TlgMainBtnColor { get; }
        /// <summary>
        /// Allows or not to work with the loyalty program.
        /// This flag is used on the basket page.
        /// </summary>
        bool IsLoyaltyProgramAvailableForProcess { get; }
        /// <summary>
        /// Shows that the order has items.
        /// </summary>
        bool HaveSelectedItemsInOrder { get; }


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
        /// Sets an agrument 'jsProcessing' if the web application starts with the Telegram.
        /// Perform the following depending on the page view:
        /// </summary>
        /// <remarks>
        /// - The page where products are selected (main page):
        /// <para>
        /// Adds a new item for a product that contains modifiers and sizes every time this method is called.
        /// Adds a new item for a product without modifiers and sizes then increases the quantity for this item.
        /// Also returns the bool value that the first item has been added.
        /// </para>
        /// - The basket page (for payment page):
        /// <para>
        /// Increases the number of positions of the selected product item.
        /// Recalculates the checkin with the allowed bonus sum to pay when the loyalty system is used.
        /// Supports operation only when working with the Telegram application, in others cases it is not recommended for use.
        /// </para>
        /// - The page where the sample item positions are changed:
        /// <para>
        /// Increases the number of positions of the selected product item.
        /// </para>
        /// - The page where modifiers and sizes of the item position are selected:
        /// <para>
        /// Increases the number of positions of the selected product item.
        /// </para>
        /// - The page where the number of the item position is inreased:
        /// <para>
        /// Adds a new item for a product without modifiers and sizes then increases the quantity for this item (It's executes for the product items
        /// that has been selected for the first time).
        /// In another case, it increases the number of positions of the selected product item.
        /// </para>
        /// </remarks>
        /// <param name="pageView"></param>
        /// <param name="jsProcessing"></param>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Task<(IOrderItemProcessing Item, bool? HasFirstItemBeenAdded)> AddOrIncreaseItemPositionAsync(PageViewType pageView,
            (IJSRuntime, ITwaNet)? jsProcessing = null, Guid? productId = null, Guid? positionId = null);

        /// <summary>
        /// Sets an agrument 'jsProcessing' if the web application starts with the Telegram.
        /// Perform the following depending on the page view:
        /// </summary>
        /// <remarks>
        /// - The page where products are selected (main page):
        /// <para>
        /// Adds a new item for a product that contains modifiers and sizes every time this method is called.
        /// Adds a new item for a product without modifiers and sizes then increases the quantity for this item.
        /// Also returns the bool value that the first item has been added.
        /// </para>
        /// - The basket page (for payment page):
        /// <para>
        /// Increases the number of positions of the selected product item.
        /// Recalculates the checkin with the allowed bonus sum to pay when the loyalty system is used.
        /// Supports operation only when working with the Telegram application, in others cases it is not recommended for use.
        /// </para>
        /// - The page where the sample item positions are changed:
        /// <para>
        /// Increases the number of positions of the selected product item.
        /// </para>
        /// - The page where modifiers and sizes of the item position are selected:
        /// <para>
        /// Increases the number of positions of the selected product item.
        /// </para>
        /// - The page where the number of the item position is inreased:
        /// <para>
        /// Adds a new item for a product without modifiers and sizes then increases the quantity for this item (It's executes for the product items
        /// that has been selected for the first time).
        /// In another case, it increases the number of positions of the selected product item.
        /// </para>
        /// </remarks>
        /// <param name="pageView"></param>
        /// <param name="jsProcessing"></param>
        /// <param name="product"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Task<(IOrderItemProcessing Item, bool? HasFirstItemBeenAdded)> AddOrIncreaseItemPositionAsync(PageViewType pageView,
            (IJSRuntime, ITwaNet)? jsProcessing = null, TransportItemDto? product = null, Guid? positionId = null);

        /// <summary>
        /// Sets an agrument 'jsProcessing' if the web application starts with the Telegram.
        /// Perform the following depending on the page view:
        /// </summary>
        /// <remarks>
        /// - The page where products are selected (main page):
        /// <para>
        /// Decreases the number of positions of the selected product wihtout modifiers and sizes.
        /// Removes the item if the number of positions is equal to zero.
        /// </para>
        /// - The basket page (for payment page):
        /// <para>
        /// Decrease the number of positions of the selected product item.
        /// Removes the item if the number of positions is equal to zero.
        /// Recalculates the checkin with the allowed bonus sum to pay when the loyalty system is used.
        /// Also:
        /// [
        /// If there are several items in the order with the same product ID (as a rule, these are products with modifiers and sizes)
        /// then just sets the current product for the order and returns the 'true'.
        /// In other case, to decrease the number of positions of the remainder product item and removes it
        /// if the number of positions is equal to zero.
        /// ]
        /// </para>
        /// - The page where the sample item positions are changed:
        /// <para>
        /// Decreases the number of positions of the selected product item and removes it if the number of positions is equal to zero.
        /// </para>
        /// - The page where modifiers and sizes of the item position are selected:
        /// <para>
        /// Decreases the number of positions of the selected product item and removes it if the number of positions is equal to zero.
        /// </para>
        /// - The page where the number of the item position is inreased:
        /// <para>
        /// Decreases the number of positions of the current product item and removes it if the number of positions is equal to zero.
        /// </para>
        /// </remarks>
        /// <param name="pageView"></param>
        /// <param name="jsProcessing"></param>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Task<(IOrderItemProcessing? Item, bool IsBasketEmpty, bool IsSeveralItemPosition)> RemoveOrDecreaseItemPositionAsync(PageViewType pageView,
            (IJSRuntime, ITwaNet)? jsProcessing = null, Guid? productId = null, Guid? positionId = null);

        /// <summary>
        /// Sets an agrument 'jsProcessing' if the web application starts with the Telegram.
        /// Perform the following depending on the page view:
        /// </summary>
        /// <remarks>
        /// - The page where products are selected (main page):
        /// <para>
        /// Decreases the number of positions of the selected product wihtout modifiers and sizes.
        /// Removes the item if the number of positions is equal to zero.
        /// </para>
        /// - The basket page (for payment page):
        /// <para>
        /// Decrease the number of positions of the selected product item.
        /// Removes the item if the number of positions is equal to zero.
        /// Recalculates the checkin with the allowed bonus sum to pay when the loyalty system is used.
        /// Also:
        /// [
        /// If there are several items in the order with the same product ID (as a rule, these are products with modifiers and sizes)
        /// then just sets the current product for the order and returns the 'true'.
        /// In other case, to decrease the number of positions of the remainder product item and removes it
        /// if the number of positions is equal to zero.
        /// ]
        /// </para>
        /// - The page where the sample item positions are changed:
        /// <para>
        /// Decreases the number of positions of the selected product item and removes it if the number of positions is equal to zero.
        /// </para>
        /// - The page where modifiers and sizes of the item position are selected:
        /// <para>
        /// Decreases the number of positions of the selected product item and removes it if the number of positions is equal to zero.
        /// </para>
        /// - The page where the number of the item position is inreased:
        /// <para>
        /// Decreases the number of positions of the current product item and removes it if the number of positions is equal to zero.
        /// </para>
        /// </remarks>
        /// <param name="pageView"></param>
        /// <param name="jsProcessing"></param>
        /// <param name="product"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        Task<(IOrderItemProcessing? Item, bool IsBasketEmpty, bool IsSeveralItemPosition)> RemoveOrDecreaseItemPositionAsync(PageViewType pageView,
            (IJSRuntime, ITwaNet)? jsProcessing = null, TransportItemDto? product = null, Guid? positionId = null);

        /// <summary>
        /// Increases the selected modifier's number of positions of the selected product item.
        /// </summary>
        /// <param name="modifierId"></param>
        /// <param name="modifierGroupId"></param>
        /// <returns></returns>
        IOrderItemProcessing IncreaseModifierItemPositionForPageWhereModifiersAndSizesOfItemPositionAreSelectedAsync(IOrderItemProcessing item,
            Guid modifierId, Guid? modifierGroupId = null);

        /// <summary>
        /// Decreases the selected modifier's number of positions of the selected product item.
        /// </summary>
        /// <param name="modifierId"></param>
        /// <param name="modifierGroupId"></param>
        /// <returns></returns>
        IOrderItemProcessing DecreaseModifierItemPositionForPageWhereModifiersAndSizesOfItemPositionAreSelectedAsync(IOrderItemProcessing item,
            Guid modifierId, Guid? modifierGroupId = null);

        /// <summary>
        /// Saves the changed personal data in API server.
        /// </summary>
        /// <returns></returns>
        Task SavePersonalDataOfOrderInServerAsync();

        /// <summary>
        /// Calculates the checkin and the allowed bonus sum to pay for the order.
        /// Sets an agrument if the web application starts with the Telegram.
        /// </summary>
        /// <param name="jsProcessing"></param>
        /// <returns></returns>
        Task CalculateCheckinAndAllowedSumAsync((IJSRuntime, ITwaNet)? jsProcessing = null);

        /// <summary>
        /// Calculates the checkin, the allowed bonus sum to pay and the available sum to pay for the order.
        /// Returns 'false' when not all necessary conditions (of loyalty program) are executed.
        /// Supports operation only when working with the Telegram application, in others cases it is not recommended for use.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        Task<bool> CalculateCheckinAndAllowedSumAndCheckAvailableMinSumForPayAsync(IJSRuntime jsRuntime, ITwaNet twaNet);

        /// <summary>
        /// Calculates the available sum to pay for the order.
        /// Returns 'false' when the order sum (to pay) doesn't match the minimum allowable payment value.
        /// Supports operation only when working with the Telegram application, in others cases it is not recommended for use.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        Task<bool> IsOrderTotalSumMoreOrEqualMinSumForPayAsync(IJSRuntime jsRuntime, ITwaNet twaNet);

        /// <summary>
        /// The order total sum with a discount
        /// </summary>
        /// <returns></returns>
        double FinalPaymentAmount();

        /// <summary>
        /// Gets the customer's wallet balance from the API server.
        /// </summary>
        /// <returns></returns>
        Task<WalletBalance> RetrieveWalletBalanceAsync();

        /// <summary>
        /// Returs 'true' if mandatory modifiers of the current item positions is selected.
        /// </summary>
        /// <returns></returns>
        bool IsSelectedModifiersOfCurrentItemPositionCorrect();

        /// <summary>
        /// Returns 'true' if the necessary data or the order is correct. Explanations of all conditions and actions are specified
        /// inside this method.
        /// Supports operation only when working with the Telegram application, in others cases it is not recommended for use.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        Task<bool> IsNecessaryDataOfOrderCorrectAsync(IJSRuntime jsRuntime, ITwaNet twaNet);

        /// <summary>
        /// Returns 'true' if the customer's wallet balance has been changed after the last request of wallet balance.
        /// </summary>
        /// <returns></returns>
        Task<bool> IsWalletBalanceChangedAfterLastRequestAsync();

        /// <summary>
        /// Tries to create the invoice link to pay the order in the Telegram payment interface.
        /// Returns 'false' when not all necessary conditions (of the order or payment) are correct.
        /// </summary>
        /// <returns></returns>
        Task<InvoiceLinkStatus> TryToCreateInvoiceUrlLinkAsync();

        /// <summary>
        /// Tries to create the invoice link to pay the order in the Telegram payment interface.
        /// Returns 'false' when not all necessary conditions (of the order or payment) are correct.
        /// Supports operation only when working with the Telegram application, in others cases it is not recommended for use.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="twaNet"></param>
        /// <returns></returns>
        Task<bool> TryToCreateInvoiceLinkAsync(IJSRuntime jsRuntime, ITwaNet twaNet);

        /// <summary>
        /// Removes the current item position from the order.
        /// </summary>
        void CancelCurrSelectedItemWithModifiers();

        /// <summary>
        /// Removes the current similar item positions from the order.
        /// </summary>
        /// <returns></returns>
        Task CancelCurrSimilarSelectedItemsWithModifiersAsync();

        /// <summary>
        /// !!! Remove this method !!!
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="twaNet"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task SetPickupTerminalWithCalculatingCheckinAsync(IJSRuntime jsRuntime, ITwaNet twaNet, Guid id);

        /// <summary>
        /// !!! Remove this method !!!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task SetPickupTerminalWithLoyaltyProgramProcessAsync(Guid id);

        /// <summary>
        /// Sets/changes the terminal of pickup.
        /// </summary>
        /// <param name="id"></param>
        void SetPickupTerminal(Guid id);

        /// <summary>
        /// Removes all selected item positions from the basket.
        /// </summary>
        /// <returns></returns>
        Task RemoveAllSelectedProductsFromBasketAsync();

        /// <summary>
        /// An information about the created order.
        /// </summary>
        /// <returns></returns>
        string InfoAboutCreatedOrderForTest();

        /// <summary>
        /// Changes the size of the current item position.
        /// </summary>
        /// <param name="sizeId"></param>
        /// <returns></returns>
        IOrderItemProcessing ChangeProductSize(Guid sizeId);

        /// <summary>
        /// Sets/changes the current group of items in the order.
        /// </summary>
        /// <param name="groupId"></param>
        void SetCurrentGroup(Guid groupId);
    }
}
