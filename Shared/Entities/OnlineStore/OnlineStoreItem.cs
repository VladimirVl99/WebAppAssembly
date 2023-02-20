using WebAppAssembly.Shared.Entities.Api.Common.Delivery;
using WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus;
using WebAppAssembly.Shared.Entities.Api.Common.OfTelegram;
using WebAppAssembly.Shared.Entities.Api.Common.OnlineStore;
using DeliveryTerminal = WebAppAssembly.Shared.Entities.Api.Common.General.Terminals.DeliveryTerminal;
using OnlineStoreItemRequest = WebAppAssembly.Shared.Entities.Api.Common.OnlineStore.OnlineStoreItem;
using Product = WebAppAssembly.Shared.Entities.OnlineStore.Orders.Menus.Product;

namespace WebAppAssembly.Shared.Entities.OnlineStore
{
    /// <summary>
    /// Common data for the online store.
    /// </summary>
    public class OnlineStoreItem : IOnlineStoreItem
    {
        #region Properties

        public IEnumerable<MenuCategory> MenuCategories { get; }

        public IEnumerable<Product> Menus { get; }

        public IEnumerable<DeliveryTerminal>? DeliveryTerminals { get; }

        public PickupType PickupType { get; }

        public bool UseIikoBizProgram { get; }

        public bool UseCoupon { get; }

        public bool UseDiscountBalance { get; }

        public float MinPaymentAmountInRubOfTg { get; }

        public TgWebAppBtnTxts TgWebAppBtnTxts { get; }

        public TgWebAppPopupMessages TgWebAppPopupMessages { get; }

        public double? TimeOutForLoyaltyProgramProcessing { get; }

        public string TgMainBtnColor { get; }

        #endregion

        #region Constructors

        public OnlineStoreItem(IEnumerable<MenuCategory> itemCategories,
            IEnumerable<Product> transportItemDtos, IEnumerable<DeliveryTerminal>? deliveryTerminals,
            PickupType pickupType, bool useIikoBizProgram, bool useCoupon, bool useDiscountBalance, float currOfRub,
            TgWebAppBtnTxts? tlgWebAppBtnTxts, TgWebAppPopupMessages tlgWebAppPopupMessages,
            double? timeOutForLoyaltyProgramProcessing, string tlgMainBtnColor)
        {
            MenuCategories = itemCategories;
            Menus = transportItemDtos;
            DeliveryTerminals = deliveryTerminals;
            PickupType = pickupType;
            UseIikoBizProgram = useIikoBizProgram;
            UseCoupon = useCoupon;
            UseDiscountBalance = useDiscountBalance;
            MinPaymentAmountInRubOfTg = currOfRub;
            TgWebAppBtnTxts = tlgWebAppBtnTxts ?? new TgWebAppBtnTxts();
            TgWebAppPopupMessages = tlgWebAppPopupMessages;
            TimeOutForLoyaltyProgramProcessing = timeOutForLoyaltyProgramProcessing;
            TgMainBtnColor = tlgMainBtnColor;
        }

        public OnlineStoreItem(OnlineStoreItemRequest response)
        {
            MenuCategories = response.MenuCategories;
            Menus = (IEnumerable<Product>)response.Menus;
            DeliveryTerminals = response.DeliveryTerminals;
            PickupType = response.PickupType;
            UseIikoBizProgram = response.UseIikoBizProgram;
            UseCoupon = response.UseCoupon;
            UseDiscountBalance = response.UseDiscountBalance;
            MinPaymentAmountInRubOfTg = response.MinPaymentAmountInRubOfTg;
            TgWebAppBtnTxts = response.TgWebAppBtnTxts ?? new TgWebAppBtnTxts();
            TgWebAppPopupMessages = response.TgWebAppPopupMessages;
            TimeOutForLoyaltyProgramProcessing = response.TimeOutForLoyaltyProgramProcessing;
            TgMainBtnColor = response.TgMainBtnColor;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets product by self ID and group ID.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Product ProductById(Guid productId, Guid? groupId = null)
            => (Product)(groupId is not null
            ? (MenuCategories.FirstOrDefault(item => item.Id == groupId)
            ?? throw new Exception(""))
            .Items?.FirstOrDefault(x => x.Id == productId)
            ?? throw new Exception("")
            : Menus.FirstOrDefault(x => x.Id == productId)
            ?? throw new Exception(""));

        #endregion
    }
}
