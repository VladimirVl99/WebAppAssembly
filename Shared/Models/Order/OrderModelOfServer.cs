using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebAppAssembly.Shared.Entities.CreateDelivery;
using WebAppAssembly.Shared.Entities.Telegram;

namespace WebAppAssembly.Shared.Models.Order
{
    public class OrderModelOfServer : ICloneable
    {
        public OrderModelOfServer() { }

        public OrderModelOfServer(Guid operationId, long chatId, List<Item> items, double totalSum, double totalAmount, double bonusSum,
            bool byCourier, Guid? terminalId, double discountSum, List<Guid> freePriceItems, List<Item> freeItems, string? comment,
            string? createdDate, DeliveryTerminal? deliveryTerminal, DeliveryPoint? address, string? coupon, int selectedBonusSum)
        {
            OperationId = operationId;
            ChatId = chatId;
            Items = items;
            TotalSum = totalSum;
            TotalAmount = totalAmount;
            BonusSum = bonusSum;
            ByCourier = byCourier;
            TerminalId = terminalId;
            DiscountSum = discountSum;
            FreePriceItems = freePriceItems;
            FreeItems = freeItems;
            Comment = comment;
            CreatedDate = createdDate;
            DeliveryTerminal = deliveryTerminal;
            Address = address;
            Coupon = coupon;
            SelectedBonusSum = selectedBonusSum;
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
        public string? CreatedDate { get; set; }
        [JsonProperty("totalAmount")]
        [JsonPropertyName("totalAmount")]
        public double TotalAmount { get; set; } = 0;
        [JsonProperty("bonusSum")]
        [JsonPropertyName("bonusSum")]
        public double BonusSum { get; set; } = 0;
        [JsonProperty("selectedBonusSum")]
        [JsonPropertyName("selectedBonusSum")]
        public int SelectedBonusSum { get; set; }
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
        [JsonProperty("freePriceItems")]
        [JsonPropertyName("freePriceItems")]
        public List<Guid> FreePriceItems { get; set; } = new List<Guid>();

        public void IncrementTotalAmount() => TotalAmount++;
        public void DecrementTotalAmount() => TotalAmount = TotalAmount == 0 ? TotalAmount : --TotalAmount;
        public void DecreaseTotalAmountBy(double i) => TotalAmount -= i;
        public bool HaveSelectedProducts() => TotalAmount != 0;
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
                if (Items is null || !Items.Any()) return;
                var amount = item.Amount;
                Items.Remove(item);
                DecreaseTotalAmountBy(amount);
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
                if (Items is null || !Items.Any()) return;

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
        public object Clone()
        {
            var items = new List<Item>();
            if (Items is null || !Items.Any()) items = new List<Item>();
            else foreach (var item in Items)
                    items.Add((Item)item.Clone());

            return new OrderModelOfServer(OperationId, ChatId, items, TotalSum, TotalAmount, BonusSum, ByCourier, TerminalId, DiscountSum, FreePriceItems, FreeItems, Comment, CreatedDate,
                DeliveryTerminal, Address, Coupon, SelectedBonusSum);
        }
    }
}
