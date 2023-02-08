using WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.RetrieveExternalMenuByID;
using WebAppAssembly.Shared.Entities.Api.Common.OnlineStore;
using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.Entities.WebApp;
using DeliveryTerminal = WebAppAssembly.Shared.Entities.Api.Common.General.Terminals.DeliveryTerminal;
using OnlineStoreItemRequest = WebAppAssembly.Shared.Entities.Api.Common.OnlineStore.OnlineStoreItem;

namespace WebAppAssembly.Shared.Entities.OnlineStore
{
    public class OnlineStoreItem : IOnlineStoreItem
    {
        #region Properties

        public IEnumerable<TransportMenuCategoryDto> MenuCategories { get; }

        public IEnumerable<TransportItemDto> Menus { get; }

        public IEnumerable<DeliveryTerminal>? DeliveryTerminals { get; }

        public PickupType PickupType { get; }

        public bool UseIikoBizProgram { get; }

        public bool UseCoupon { get; }

        public bool UseDiscountBalance { get; }

        public float MinPaymentAmountInRubOfTg { get; }

        public TlgWebAppBtnTxts TgWebAppBtnTxts { get; }

        public TlgWebAppPopupMessages TgWebAppPopupMessages { get; }

        public double? TimeOutForLoyaltyProgramProcessing { get; }

        public string TgMainBtnColor { get; }

        #endregion

        #region Constructors

        public OnlineStoreItem(IEnumerable<TransportMenuCategoryDto> itemCategories,
            IEnumerable<TransportItemDto> transportItemDtos, IEnumerable<DeliveryTerminal>? deliveryTerminals,
            PickupType pickupType, bool useIikoBizProgram, bool useCoupon, bool useDiscountBalance, float currOfRub,
            TlgWebAppBtnTxts? tlgWebAppBtnTxts, TlgWebAppPopupMessages tlgWebAppPopupMessages,
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
            TgWebAppBtnTxts = tlgWebAppBtnTxts ?? new TlgWebAppBtnTxts();
            TgWebAppPopupMessages = tlgWebAppPopupMessages;
            TimeOutForLoyaltyProgramProcessing = timeOutForLoyaltyProgramProcessing;
            TgMainBtnColor = tlgMainBtnColor;
        }

        public OnlineStoreItem(OnlineStoreItemRequest response)
        {
            MenuCategories = response.MenuCategories;
            Menus = response.Menus;
            DeliveryTerminals = response.DeliveryTerminals;
            PickupType = response.PickupType;
            UseIikoBizProgram = response.UseIikoBizProgram;
            UseCoupon = response.UseCoupon;
            UseDiscountBalance = response.UseDiscountBalance;
            MinPaymentAmountInRubOfTg = response.MinPaymentAmountInRubOfTg;
            TgWebAppBtnTxts = response.TgWebAppBtnTxts ?? new TlgWebAppBtnTxts();
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
        public TransportItemDto ProductById(Guid productId, Guid? groupId = null)
            => groupId is not null
            ? (MenuCategories.FirstOrDefault(item => item.Id == groupId)
            ?? throw new Exception(""))
            .Items.FirstOrDefault(x => x.ItemId == productId)
            ?? throw new Exception("")
            : Menus.FirstOrDefault(x => x.ItemId == productId)
            ?? throw new Exception("");

        #endregion
    }
}
