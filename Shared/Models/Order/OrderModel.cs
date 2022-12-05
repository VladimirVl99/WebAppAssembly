using ApiServerForTelegram.Entities.EExceptions;
using ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Org.BouncyCastle.Asn1.X9;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;
using WebAppAssembly.Shared.Entities;
using WebAppAssembly.Shared.Entities.CreateDelivery;
using WebAppAssembly.Shared.Entities.Telegram;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace WebAppAssembly.Shared.Models.Order
{
    public class OrderModel : ICloneable
    {
        public OrderModel()
        {
            OperationId = Guid.NewGuid();
            CreatedDate = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss");
            Items = new List<Item>();
        }

        public OrderModel(Guid operationId, double totalSum, string comment, string createdDate)
        {
            OperationId = operationId;
            TotalSum = totalSum;
            Comment = comment;
            CreatedDate = createdDate;
        }

        public OrderModel(List<Item> items)
        {
            OperationId = Guid.NewGuid();
            Items = items;
            CreatedDate = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss");
        }

        public OrderModel(Guid operationId, List<Item> items, string comment, double totalSum, string createdDate, double totalAmount, double bonusSum, bool byCourier, Guid? terminalId)
        {
            OperationId = operationId;
            Items = items;
            Comment = comment;
            TotalSum = totalSum;
            CreatedDate = createdDate;
            TotalAmount = totalAmount;
            WalletBalance = bonusSum;
            ByCourier = byCourier;
            TerminalId = terminalId;
        }

        public OrderModel(Guid operationId, long chatId, List<Item> items, List<Item> freeItems, string comment, double totalSum, string createdDate, double totalAmount, double bonusSum, bool byCourier, Guid? terminalId, DeliveryTerminal deliveryTerminal, DeliveryPoint address, string coupon, double discountSum, List<Guid> freePriceItems)
        {
            OperationId = operationId;
            ChatId = chatId;
            Items = items;
            FreeItems = freeItems;
            Comment = comment;
            TotalSum = totalSum;
            CreatedDate = createdDate;
            TotalAmount = totalAmount;
            WalletBalance = bonusSum;
            ByCourier = byCourier;
            TerminalId = terminalId;
            DeliveryTerminal = deliveryTerminal;
            Address = address;
            Coupon = coupon;
            DiscountSum = discountSum;
            DiscountFreeItems = freePriceItems;
        }

        public OrderModel(Guid operationId, long chatId, List<Item> items, double totalSum, double totalAmount, double bonusSum, bool byCourier,
            Guid? terminalId, double discountSum, List<Guid> freePriceItems, List<Item> freeItems, string? comment = null, string? createdDate = null,
            DeliveryTerminal? deliveryTerminal = null, DeliveryPoint? address = null, string? coupon = null)
        {
            OperationId = operationId;
            ChatId = chatId;
            Items = items;
            FreeItems = freeItems;
            Comment = comment;
            TotalSum = totalSum;
            CreatedDate = createdDate;
            TotalAmount = totalAmount;
            WalletBalance = bonusSum;
            ByCourier = byCourier;
            TerminalId = terminalId;
            DeliveryTerminal = deliveryTerminal;
            Address = address;
            Coupon = coupon;
            DiscountSum = discountSum;
            DiscountFreeItems = freePriceItems;
        }

        [JsonProperty("operationId")]
        [JsonPropertyName("operationId")]
        public Guid OperationId { get; set; }
        [JsonProperty("chatId")]
        [JsonPropertyName("chatId")]
        public long ChatId { get; set; }
        [JsonProperty("items")]
        [JsonPropertyName("items")]
        public List<Item>? Items { get; set; }
        [JsonProperty("freeItems")]
        [JsonPropertyName("freeItems")]
        public List<Item> FreeItems { get; set; } = new List<Item>();
        [JsonProperty("comment")]
        [JsonPropertyName("comment")]
        public string? Comment { get; set; }
        [JsonProperty("totalSum")]
        [JsonPropertyName("totalSum")]
        public double TotalSum { get; set; } = 0;
        [JsonProperty("createdDate")]
        [JsonPropertyName("createdDate")]
        public string? CreatedDate { get; private set; }
        [JsonProperty("totalAmount")]
        [JsonPropertyName("totalAmount")]
        public double TotalAmount { get; set; } = 0;
        [JsonProperty("bonusSum")]
        [JsonPropertyName("bonusSum")]
        public double WalletBalance { get; set; } = 0;
        [JsonIgnore]
        public int AllowedWalletSum { get; set; } = 0;
        [JsonIgnore]
        public double? AvailableWalletSum { get; set; }
        [JsonProperty("selectedBonusSum")]
        [JsonPropertyName("selectedBonusSum")]
        public int SelectedWalletSum { get; set; } = 0;
        [JsonProperty("byCourier")]
        [JsonPropertyName("byCourier")]
        public bool ByCourier { get; set; } = true;
        [JsonProperty("terminalId")]
        [JsonPropertyName("terminalId")]
        public Guid? TerminalId { get; set; }
        [JsonProperty("deliveryTerminal")]
        [JsonPropertyName("deliveryTerminal")]
        public DeliveryTerminal? DeliveryTerminal { get; set; }
        [JsonProperty("address")]
        [JsonPropertyName("address")]
        public DeliveryPoint? Address { get; set; }
        [JsonProperty("coupon")]
        [JsonPropertyName("coupon")]
        public string? Coupon { get; set; }
        [JsonProperty("discountSum")]
        [JsonPropertyName("discountSum")]
        public double DiscountSum { get; set; } = 0;
        [JsonProperty("finalSum")]
        [JsonPropertyName("finalSum")]
        public double FinalSum { get; set; }
        [JsonProperty("discountProcent")]
        [JsonPropertyName("discountProcent")]
        public double DiscountProcent { get; set; }
        [JsonProperty("discountFreeItems")]
        [JsonPropertyName("discountFreeItems")]
        public List<Guid> DiscountFreeItems { get; set; } = new List<Guid>();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public double SelectedTotalAmountByItemId(Guid itemId)
        {
            double totalAmount = 0;
            var items = CurrItems().Where(item => item.ProductId == itemId);
            foreach (var item in items)
                totalAmount += item.Amount;
            return totalAmount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double IncrementTotalAmount() => ++TotalAmount;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double DecrementTotalAmount() => TotalAmount = TotalAmount == 0 ? TotalAmount : --TotalAmount;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        public double IncrementTotalAmountWithPrice(Item item)
        {
            var sum = item.IncrementAmountWithPrice();
            IncrementTotalAmount();
            return IncreaseTotalSum(sum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        public double DecrementTotalAmountWithPrice(Item item)
        {
            var sum = item.DecrementAmountWithPrice();
            var amount = DecrementTotalAmount();
            if (amount <= 0) return TotalSum = 0;
            return DecreaseTotalSum(sum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="modifierId"></param>
        /// <param name="modifierGroupId"></param>
        /// <returns></returns>
        public double IncrementTotalAmountOfModifierWithPrice(Item item, Guid modifierId, Guid? modifierGroupId)
        {
            var sum = item.IncreaseAmountOfModifier(modifierId, modifierGroupId);
            return IncreaseTotalSum(sum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="modifierId"></param>
        /// <param name="modifierGroupId"></param>
        /// <returns></returns>
        public double DecrementTotalAmountOfModifierWithPrice(Item item, Guid modifierId, Guid? modifierGroupId)
        {
            var sum = item.DecreaseAmountOfModifier(modifierId, modifierGroupId);
            return DecreaseTotalSum(sum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private double DecreaseTotalAmountBy(double i) => TotalAmount = TotalAmount == 0 ? TotalAmount : TotalAmount - i;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        private double DecreaseTotalSumBy(double sum) => TotalSum -= sum;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        public double DecreaseTotalAmountAndSumBy(double amount, double sum)
        {
            var res = DecreaseTotalAmountBy(amount);
            if (res <= 0) return TotalSum = 0;
            return DecreaseTotalSumBy(sum);
        }

        public bool HaveSelectedProducts() => TotalAmount > 0;
        public double TotalAmountByProduct(Guid id)
        {
            if (Items is null || !Items.Any())
                return 0;
            double total = 0;
            var items = Items.Where(x => x.ProductId == id);
            foreach (var item in items)
                total += item.Amount;
            return total;
        }
        public void ZeroAmountOfItem(Item item)
        {
            try
            {
                if (Items is null) return;
                var amount = item.Amount;
                var totalPrice = item.TotalPrice ?? throw new InfoException(typeof(OrderModel).FullName!, nameof(Exception),
                    $"{typeof(Item).FullName!}.{nameof(Item.TotalPrice)}", ExceptionType.Null); ;
                Items.Remove(item);
                DecreaseTotalAmountAndSumBy(amount, totalPrice);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(OrderModel).FullName}.{nameof(ZeroAmountOfItem)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
            }
        }
        public void RemoveItemsById(Guid productId)
        {           
            try
            {
                if (Items is null) return;

                var items = new List<Item>(Items.Where(x => x.ProductId == productId));
                foreach (var item in items) ZeroAmountOfItem(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(OrderModel).FullName}.{nameof(RemoveItemsById)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
            }
        }
        public void WithNewParameters()
        {
            OperationId = Guid.NewGuid();
            CreatedDate = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss");
            TotalSum = 0;
            TotalAmount = 0;
            Items ??= new List<Item>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        public double IncreaseTotalSum(double sum) => TotalSum += sum;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        public double DecreaseTotalSum(double sum) => TotalSum -= sum;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public IEnumerable<Item> CurrItems()
            => Items ?? throw new InfoException(typeof(OrderModel).FullName!, nameof(CurrItems),
                nameof(Exception), $"{nameof(Enumerable)}<{typeof(Item).FullName!}>", ExceptionType.Null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public Item ItemById(Guid productId)
            => CurrItems().FirstOrDefault(item => item.ProductId == productId) ?? throw new InfoException(typeof(OrderModel).FullName!,
                nameof(ItemById), nameof(Exception), $"No found an item ({typeof(Item).FullName}) by ProductId - '{productId}'");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Item? ItemByIdOrDefault(Guid productId)
            => CurrItems().FirstOrDefault(item => item.ProductId == productId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public Item ItemById(Guid productId, Guid? positionId = null)
            => positionId is null ? ItemById(productId) : CurrItems().FirstOrDefault(item => item.ProductId == productId && item.PositionId == positionId)
            ?? throw new InfoException(typeof(OrderModel).FullName!, nameof(ItemById), nameof(Exception),
                $"No found an item ({typeof(Item).FullName}) by ProductId - '{productId}' and PositionId - '{positionId}'");

        public object Clone()
        {
            var items = new List<Item>();
            if (Items is null || !Items.Any()) items = new List<Item>();
            else foreach (var item in Items)
                    items.Add((Item)item.Clone());

            return new OrderModel(OperationId, ChatId, items, TotalSum, TotalAmount, WalletBalance, ByCourier, TerminalId, DiscountSum, DiscountFreeItems, FreeItems, Comment, CreatedDate, 
                DeliveryTerminal, Address, Coupon);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool HaveMoreThanOneItemPositionOfProduct(Guid productId)
        {
            var items = CurrItems().Where(x => x.ProductId == productId);

            int count = 0;
            foreach (var item in items) if (count++ > 0) return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="priceOfItemSize"></param>
        public void ChangeItemsSize(Item item, float priceOfItemSize)
        {
            var different = item.ChangePriceOfItem(priceOfItemSize);
            IncreaseTotalSum(different);
        }
    }
}