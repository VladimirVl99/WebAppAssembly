using WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders;
using WebAppAssembly.Shared.Entities.OnlineStore.Orders;
using WebAppAssembly.Shared.Repositories.Common;
using Order = WebAppAssembly.Shared.Entities.OnlineStore.Orders.Order;
using OrderRequest = WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders.Order;

namespace WebAppAssembly.Shared.Models.OrderData
{
    public class OrderClient : Order, IOrderClientService
    {
        #region Properties

        public int AllowedWalletAmount { get; private set; }

        public double? AvailableWalletAmount { get; private set; }

        #endregion

        #region Constructors

        public OrderClient()
            : base(Guid.NewGuid())
        { }

        public OrderClient(Order self)
            : base(self.OperationId, self.Items, self.Comment, self.PaymentAmountOfSeletedItems,
                  self.CreatedDate, self.NumberOfSelectedItems, self.SelectedNumberOfBonuses)
        { }

        public OrderClient(OrderRequest respnose)
            : base(respnose.OperationId, respnose.Items.ToOrderItems(), respnose.Comment,
                  respnose.PaymentAmountOfSeletedItems, respnose.CreatedDate,
                  respnose.NumberOfSelectedItems, respnose.SelectedNumberOfBonuses ?? 0)
        { }

        #endregion

        #region Methods

        public override void ResetWalletAmounts()
        {
            base.ResetWalletAmounts();
            AvailableWalletAmount = null;
            AllowedWalletAmount = 0;
        }

        public void SetAvailableWalletAmount(double? amount)
            => AvailableWalletAmount = amount;

        public void SetAllowedWalletAmount(int amount)
            => AllowedWalletAmount = amount;

        public void ResetAllowedWalletAmount()
            => AllowedWalletAmount = 0;

        #endregion
    }
}
