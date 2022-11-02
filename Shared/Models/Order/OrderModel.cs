﻿using Newtonsoft.Json;
using WebAppAssembly.Shared.Entities.CreateDelivery;
using WebAppAssembly.Shared.Entities.Telegram;

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
            BonusSum = bonusSum;
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
            BonusSum = bonusSum;
            ByCourier = byCourier;
            TerminalId = terminalId;
            DeliveryTerminal = deliveryTerminal;
            Address = address;
            Coupon = coupon;
            DiscountSum = discountSum;
            FreePriceItems = freePriceItems;
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
            BonusSum = bonusSum;
            ByCourier = byCourier;
            TerminalId = terminalId;
            DeliveryTerminal = deliveryTerminal;
            Address = address;
            Coupon = coupon;
            DiscountSum = discountSum;
            FreePriceItems = freePriceItems;
        }

        [JsonProperty("operationId")]
        public Guid OperationId { get; private set; }
        public long ChatId { get; set; }
        [JsonProperty("items")]
        public List<Item>? Items { get; set; }
        public List<Item> FreeItems { get; set; } = new List<Item>();
        [JsonProperty("comment")]
        public string? Comment { get; set; }
        [JsonProperty("totalSum")]
        public double TotalSum { get; set; } = 0;
        [JsonProperty("createdDate")]
        public string? CreatedDate { get; private set; }
        [JsonProperty("totalAmount")]
        public double TotalAmount { get; set; } = 0;
        [JsonProperty("bonusSum")]
        public double BonusSum { get; private set; } = 0;
        [JsonProperty("byCourier")]
        public bool ByCourier { get; set; } = true;
        [JsonProperty("terminalId")]
        public Guid? TerminalId { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public DeliveryTerminal? DeliveryTerminal { get; set; }
        [JsonProperty("address")]
        public DeliveryPoint? Address { get; set; }
        public string? Coupon { get; set; }
        public double DiscountSum { get; set; } = 0;
        public double FinalSum { get; set; }
        public double DiscountProcent { get; set; }
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

            return new OrderModel(OperationId, ChatId, items, TotalSum, TotalAmount, BonusSum, ByCourier, TerminalId, DiscountSum, FreePriceItems, FreeItems, Comment, CreatedDate, 
                DeliveryTerminal, Address, Coupon);
        }
    }
}