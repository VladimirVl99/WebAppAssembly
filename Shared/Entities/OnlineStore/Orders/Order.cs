using ApiServerForTelegram.Entities.EExceptions;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.RetrieveExternalMenuByID;
using WebAppAssembly.Shared.Models.OrderData;

namespace WebAppAssembly.Shared.Entities.OnlineStore.Orders
{
    public class Order : IOrderService
    {
        #region Fields

        private protected ICollection<IOrderItemProcessing>? _items;
        private protected ICollection<IOrderItemProcessing>? _freeItems;
        private protected ICollection<Guid>? _discountFreeItems;

        #endregion

        #region Properties

        public Guid OperationId { get; private protected set; }

        public ICollection<IOrderItemProcessing> Items
        {
            get => _items ?? new List<IOrderItemProcessing>();
            private protected set => _items = value;
        }

        public ICollection<IOrderItemProcessing> FreeItems
        {
            get => _freeItems ?? new List<IOrderItemProcessing>();
            private protected set => _freeItems = value;
        }

        public string? Comment { get; set; }

        public double PaymentAmountOfSeletedItems { get; private protected set; }

        public string? CreatedDate { get; private protected set; }

        public double NumberOfSelectedItems { get; private protected set; }

        public double WalletBalance { get; private protected set; }

        public int SelectedNumberOfBonuses { get; private protected set; }

        public string? Coupon { get; set; }

        public double DiscountAmount { get; private protected set; }

        public double TotalPaymentAmount { get; private protected set; }

        public double DiscountProcent { get; private protected set; }

        public ICollection<Guid> DiscountFreeItems
        {
            get => _discountFreeItems ?? new List<Guid>();
            private protected set => _discountFreeItems = value;
        }

        public bool HaveSelectedItems
            => Items.Any();

        #endregion

        #region Constructors

        /// <summary>
        /// Call this constructor if you need to create a new order.
        /// </summary>
        /// <param name="id"></param>
        public Order(Guid id)
        {
            OperationId = id;
            CreatedDate = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss");
        }

        public Order(Guid operationId, ICollection<IOrderItemProcessing> items, string? comment, 
            double paymentAmountOfSelectedItems, string? createdDate, double numberOfSelectedItems,
            int selectedNumberOfBonuses)
        {
            OperationId = operationId;
            _items = items;
            Comment = comment;
            PaymentAmountOfSeletedItems = paymentAmountOfSelectedItems;
            CreatedDate = createdDate;
            NumberOfSelectedItems = numberOfSelectedItems;
            SelectedNumberOfBonuses = selectedNumberOfBonuses;
        }

        #endregion

        #region Methods

        #region Public

        public void ResetDiscountItems()
        {
            FreeItems.Clear();
            DiscountFreeItems.Clear();
        }

        public virtual void ResetWalletAmounts()
        {
            SelectedNumberOfBonuses = 0;
        }

        public void ResetDiscountItemsWithDiscountAmounts()
        {
            ResetDiscountItems();
            DiscountAmount = 0;
            DiscountProcent = 0;
            ResetFinalPaymentAmount();
        }

        public void SetDiscountAmount(double amount)
        {
            DiscountAmount = amount;
            DiscountProcent = DiscountAmount * 100 / PaymentAmountOfSeletedItems;
            TotalPaymentAmount = PaymentAmountOfSeletedItems - DiscountAmount;
        }

        public void SetDiscountFreeItems(List<Guid> guids)
            => DiscountFreeItems = guids;

        public void SetFreeItems(List<IOrderItemProcessing> items)
            => FreeItems = items;

        public void ResetFinalPaymentAmount()
            => TotalPaymentAmount = PaymentAmountOfSeletedItems;

        public void ResetTotalPaymentAmount()
            => TotalPaymentAmount = NumberOfSelectedItems = PaymentAmountOfSeletedItems = 0;

        public void ClearBasketOfOrder()
        {
            ResetTotalPaymentAmount();
            Items.Clear();
        }

        public void SetSelectedWalletAmount(int amount)
            => SelectedNumberOfBonuses = amount;

        public void SetWalletBalance(double walletBalance)
            => WalletBalance = walletBalance;

        public double TotalNumberOfSelectedItemsById(Guid itemId)
        {
            double totalAmount = 0;
            var items = Items.Where(item => item.ProductId == itemId);
            foreach (var item in items)
                totalAmount += item.Amount;
            return totalAmount;
        }

        public double FinalPaymentAmountWithSelectedWalletAmount()
            => SelectedNumberOfBonuses > 0 ? TotalPaymentAmount - SelectedNumberOfBonuses : TotalPaymentAmount;

        public double IncreaseTotalNumberOfItemAndTotalPaymentAmount(IOrderItemProcessing item)
        {
            var sum = item.IncreaseQuantityAndPrice();
            IncreaseTotalNumberOfSelectedItems();
            return IncreaseTotalPaymentAmount(sum);
        }

        public double DecreaseTotalNumberOfItemAndTotalPaymentAmount(IOrderItemProcessing item)
        {
            var sum = item.DecrementQuantityWithPrice();
            var amount = DecreaseTotalNumberOfSelectedItems();
            if (amount <= 0) return PaymentAmountOfSeletedItems = 0;
            return DecreaseTotalPaymentAmount(sum);
        }

        public double IncreaseTotalNumberOfModifierAndTotalPaymentAmount(IOrderItemProcessing item, Guid modifierId, Guid? modifierGroupId)
        {
            var (changedPriceBy, _) = item.IncreaseQuantityWithPriceOfModifier(modifierId, modifierGroupId);
            return IncreaseTotalPaymentAmount(changedPriceBy);
        }

        public double DecreaseTotalNumberOfModifierAndTotalPaymentAmount(IOrderItemProcessing item, Guid modifierId, Guid? modifierGroupId)
        {
            var (changedPriceBy, _) = item.DecreaseQuantityWithPriceOfModifier(modifierId, modifierGroupId);
            return DecreaseTotalPaymentAmount(changedPriceBy);
        }

        public void ZeroAmountOfItem(IOrderItemProcessing item)
        {
            try
            {
                var amount = item.Amount;
                var totalPrice = item.TotalPrice;
                Items.Remove(item);
                DecreaseTotalNumberOfItemsAndTotalPaymentAmountBy(amount, totalPrice);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(Order).FullName}.{nameof(ZeroAmountOfItem)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
            }
        }

        public void RemoveItemsById(Guid productId)
        {
            try
            {
                var items = new List<IOrderItemProcessing>(Items.Where(x => x.ProductId == productId));
                foreach (var item in items) ZeroAmountOfItem(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(Order).FullName}.{nameof(RemoveItemsById)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
            }
        }

        public double IncreaseTotalPaymentAmount(double sum)
            => PaymentAmountOfSeletedItems += sum;

        public double DecreaseTotalPaymentAmount(double sum)
            => PaymentAmountOfSeletedItems > 0 ? PaymentAmountOfSeletedItems -= sum : PaymentAmountOfSeletedItems = 0;

        public IOrderItemProcessing FirstItemById(Guid productId)
            => Items.FirstOrDefault(item => item.ProductId == productId) ?? throw new InfoException(typeof(Order).FullName!,
                nameof(FirstItemById), nameof(Exception), $"No found an item ({typeof(OrderItem).FullName}) by product ID - '{productId}'");

        public IOrderItemProcessing? FirstItemByIdOrDefault(Guid productId)
            => Items.FirstOrDefault(item => item.ProductId == productId);

        public IOrderItemProcessing ItemById(Guid productId, Guid? positionId = null)
            => positionId is null ? FirstItemById(productId) : Items.FirstOrDefault(item => item.ProductId == productId && item.PositionId == positionId)
            ?? throw new InfoException(typeof(Order).FullName!, nameof(ItemById), nameof(Exception),
                $"No found an item ({typeof(OrderItem).FullName}) by ProductId - '{productId}' and PositionId - '{positionId}'");

        public bool HaveSampleItemPositions(Guid productId)
        {
            var items = Items.Where(x => x.ProductId == productId);

            int count = 0;
            foreach (var item in items) if (count++ > 0) return true;
            return false;
        }

        public void ChangeSizeOfItem(IOrderItemProcessing item, float priceOfItemSize, Guid sizeId)
        {
            var different = item.ChangeSize(sizeId, priceOfItemSize);
            IncreaseTotalPaymentAmount(different);
        }

        public IOrderItemProcessing AddItemWithNewPosition(TransportItemDto product, Guid? sizeId = null)
        {
            var item = new OrderItem(product, Guid.NewGuid(), sizeId);
            Items!.Add(item);
            return item;
        }

        #endregion

        #region Private

        /// <summary>
        /// Increases the total number of selected items.
        /// </summary>
        /// <returns></returns>
        private double IncreaseTotalNumberOfSelectedItems()
            => ++NumberOfSelectedItems;

        /// <summary>
        /// Decreases the total number of selected items.
        /// </summary>
        /// <returns></returns>
        private double DecreaseTotalNumberOfSelectedItems()
            => NumberOfSelectedItems = NumberOfSelectedItems == 0 ? NumberOfSelectedItems : --NumberOfSelectedItems;

        /// <summary>
        /// Decreases the total number of items from the basket.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private double DecreaseTotalNumberOfItemsBy(double x)
            => NumberOfSelectedItems = NumberOfSelectedItems == 0 ? NumberOfSelectedItems : NumberOfSelectedItems - x;

        /// <summary>
        /// Decreases the total payment amount.
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        private double DecreaseTotalPaymentAmountBy(double sum)
            => PaymentAmountOfSeletedItems -= sum;

        /// <summary>
        /// Decreases the total number of items from the basket and decreases the total payment amount.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        private double DecreaseTotalNumberOfItemsAndTotalPaymentAmountBy(double amount, double sum)
        {
            var res = DecreaseTotalNumberOfItemsBy(amount);
            if (res <= 0) return PaymentAmountOfSeletedItems = 0;
            return DecreaseTotalPaymentAmountBy(sum);
        }

        #endregion

        #endregion
    }
}
