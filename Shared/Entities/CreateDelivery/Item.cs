using WebAppAssembly.Shared.Entities.EMenu;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json.Serialization;
using ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID;
using ApiServerForTelegram.Entities.EExceptions;
using WebAppAssembly.Shared.Entities.IikoCloudApi;
using System.Text.RegularExpressions;
using WebAppAssembly.Shared.Entities.Telegram;

namespace WebAppAssembly.Shared.Entities.CreateDelivery
{
    public class Item : ICloneable
    {
        public Item() { }
        public Item(Guid productId, string productName, string type, double amount, double? price = default, Guid? productSizeId = default,
            ComboInformation? comboInformation = default, Guid? positionId = default, IEnumerable<Modifier>? modifiers = default,
            string? comment = default)
        {
            ProductId = productId;
            ProductName = productName;
            Type = type;
            Amount = amount;
            ProductSizeId = productSizeId;
            ComboInformation = comboInformation;
            PositionId = positionId;
            Modifiers = modifiers;
            Comment = comment;
            Price = price;
        }

        public Item(TransportItemDto product, Guid? positionId = default)
        {
            ProductId = product.ItemId ?? throw new InfoException(typeof(Item).FullName!,
                    nameof(Exception), $"{typeof(TransportItemDto).FullName!}.{nameof(TransportItemDto.ItemId)}", ExceptionType.Null);
            ProductName = product.Name ?? throw new InfoException(typeof(Item).FullName!,
                    nameof(Exception), $"{typeof(TransportItemDto).FullName!}.{nameof(TransportItemDto.Name)}", ExceptionType.Null);
            Type = product.OrderItemType;
            PositionId = positionId != default && positionId != Guid.Empty ? positionId : Guid.NewGuid();
            Price = product.Price();

            var modifierList = new List<Modifier>();
            var simpleGroups = new List<SimpleGroupModifier>();
            var simpleModifiers = new List<SimpleModifier>();

            if (product.ItemSizes is not null)
            {
                foreach (var size in product.ItemSizes)
                {
                    if (size.ItemModifierGroups is not null)
                    {
                        foreach (var modifierGroup in size.ItemModifierGroups)
                        {
                            if (modifierGroup.Items is not null)
                            {
                                int totalDefault = 0;
                                foreach (var item in modifierGroup.Items)
                                {
                                    AddSimpleModifier(ref modifierList, ref simpleModifiers, item, modifierGroup.ItemGroupId);
                                    totalDefault += item.Restrictions?.ByDefault ?? 0;
                                }
                                AddSimpleGroupModifier(ref simpleGroups, modifierGroup, totalDefault);      
                            }
                        }    
                    }
                }
                SimpleGroupModifiers = simpleGroups;
                Modifiers = modifierList.Any() ? modifierList : null;
                SimpleModifiers = simpleModifiers;
            }
        }

        public Item(string productName, Guid productId, IEnumerable<Modifier>? modifiers, double? price, Guid? positionId, string type, double amount,
            Guid? productSizeId, ComboInformation? comboInformation, string? comment, IEnumerable<SimpleGroupModifier>? simpleGroupModifiers,
            IEnumerable<SimpleModifier>? simpleModifiers)
        {
            ProductName = productName;
            ProductId = productId;
            Modifiers = modifiers;
            Price = price;
            PositionId = positionId;
            Type = type;
            Amount = amount;
            ProductSizeId = productSizeId;
            ComboInformation = comboInformation;
            Comment = comment;
            SimpleGroupModifiers = simpleGroupModifiers;
            SimpleModifiers = simpleModifiers;
        }

        public Item(Guid productId, string productName)
        {
            ProductId = productId;
            ProductName = productName;
        }

        [JsonProperty("productName")]
        [JsonPropertyName("productName")]
        public string? ProductName { get; set; }
        // ID of menu item
        // Can be obtained by /api/1/nomenclature operation
        [JsonRequired]
        [JsonProperty("productId")]
        [JsonPropertyName("productId")]
        public Guid ProductId { get; set; }
        // Modifiers
        [JsonProperty("modifiers")]
        [JsonPropertyName("modifiers")]
        public IEnumerable<Modifier>? Modifiers { get; set; }
        // Price per item unit. Can be sent different from the price in the base menu
        [JsonProperty("price")]
        [JsonPropertyName("price")]
        public double? Price { get; set; }
        [JsonProperty("totalPrice")]
        [JsonPropertyName("totalPrice")]
        public double? TotalPrice { get; set; }
        // Unique identifier of the item in the order. MUST be unique for the whole system. Therefore it must be generated with Guid.NewGuid()
        // If sent null, it generates automatically on iikoTransport side
        [JsonProperty("positionId")]
        [JsonPropertyName("positionId")]
        public Guid? PositionId { get; set; }
        // Product or Compound
        [JsonRequired]
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        // Quantity [ 0 .. 999.999 ]
        [JsonRequired]
        [JsonProperty("amount")]
        [JsonPropertyName("amount")]
        public double Amount { get; set; } = 0;
        // Size ID. Required if a stock list item has a size scale
        [JsonProperty("productSizeId")]
        [JsonPropertyName("productSizeId")]
        public Guid? ProductSizeId { get; set; }
        // Combo details if combo includes order item
        [JsonProperty("comboInformation")]
        [JsonPropertyName("comboInformation")]
        public ComboInformation? ComboInformation { get; set; }
        // Comment [ 0 .. 255 ] characters
        [JsonProperty("comment")]
        [JsonPropertyName("comment")]
        public string? Comment { get; set; }
        [JsonProperty("simpleGroupModifiers")]
        [JsonPropertyName("simpleGroupModifiers")]
        public IEnumerable<SimpleGroupModifier>? SimpleGroupModifiers { get; set; }
        [JsonProperty("simpleModifiers")]
        [JsonPropertyName("simpleModifiers")]
        public IEnumerable<SimpleModifier>? SimpleModifiers { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        private double IncrementTotalPrice()
        {
            TotalPrice ??= 0;
            var price = PriceBy();
            TotalPrice += price;
            return price;
        }

        private double DecrementTotalPrice()
        {
            TotalPrice ??= 0;
            var price = PriceBy();
            TotalPrice = Amount == 0 ? 0 : TotalPrice -= price;
            return price;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double IncrementAmount()
        {
            Amount++;
            return IncrementTotalPrice();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double DecrementAmount()
        {
            if (Amount <= 0) return 0;
            --Amount;
            return DecrementTotalPrice();
        }
        public IEnumerable<Modifier> GetModifiers() => Modifiers is not null ? Modifiers : Enumerable.Empty<Modifier>();
        public bool IsSelectedModifier(Guid modifierId, Guid? productGroupId = null, int? trying = null)
        {
            try
            {
                return productGroupId is not null && productGroupId != Guid.Empty
                    ? (int)GetModifiers().First(x => x.ProductId == modifierId && x.ProductGroupId == productGroupId).Amount != 0
                    : (int)GetModifiers().First(x => x.ProductId == modifierId).Amount != 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(Item).FullName}.{nameof(IsSelectedModifier)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
                return false;
                //if (trying != null)
                //{
                //    if (trying < 50) ++trying;
                //    else return false; // ???
                //}
                //else trying = 0;
                //Task.Delay(1);
                //return IsSelectedModifier(ModifierId, productGroupId, trying);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifier"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        private double IncrementTotalPriceByModifierPrice(Modifier modifier)
        {
            TotalPrice ??= 0;
            return (double)(TotalPrice += modifier.Price ?? throw new InfoException(typeof(Item).FullName!, nameof(IncrementTotalPriceByModifierPrice),
                nameof(Exception), $"Price of an modifier by ID - '{modifier.ProductId}' can't be null"));
        }

        private double DecrementTotalPriceByModifierPrice(Modifier modifier)
        {
            TotalPrice ??= 0;
            return (double)(TotalPrice -= modifier.Price ?? throw new InfoException(typeof(Item).FullName!, nameof(DecrementTotalPriceByModifierPrice),
                nameof(Exception), $"Price by an modifier ID - '{ProductId}' can't be null"));
        }

        /// <summary>
        /// !!! Need to save it from crash !!!
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productGroupId"></param>
        /// <returns></returns>
        public double IncreaseAmountOfModifier(Guid id, Guid? productGroupId = null)
        {
            var modifier = ModifierById(id);
            modifier.Amount++;

            if (productGroupId is not null && productGroupId != Guid.Empty && SimpleGroupModifiers is not null)
            {
                var groupModifier = SimpleGroupModifiers.First(x => x.Id == productGroupId);
                groupModifier.MaxAmount--;
                groupModifier.MinAmount--;
            }
            else if (SimpleModifiers is not null)
            {
                var simpleModifier = SimpleModifiers.First(x => x.Id == id);
                simpleModifier.MaxAmount--;
                simpleModifier.MinAmount--;
            }
            return IncrementTotalPriceByModifierPrice(modifier);
        }

        /// <summary>
        /// !!! Need to save it from crash !!!
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productGroupId"></param> 
        /// <returns></returns>
        public double DecreaseAmountOfModifier(Guid id, Guid? productGroupId = default)
        {
            var modifier = ModifierById(id);
            if (modifier.Amount <= 0) return (double)TotalPrice!;
            modifier.Amount--;

            if (productGroupId is not null && productGroupId != Guid.Empty && SimpleGroupModifiers is not null)
            {
                var groupModifier = SimpleGroupModifiers.First(x => x.Id == productGroupId);
                groupModifier.MaxAmount++;
                groupModifier.MinAmount++;
            }
            else if (SimpleModifiers is not null)
            {
                var simpleModifier = SimpleModifiers.First(x => x.Id == id);
                simpleModifier.MaxAmount++;
                simpleModifier.MinAmount++;
            }
            return DecrementTotalPriceByModifierPrice(modifier);
        }
        public double AmountOfModifier(Guid id, Guid? productGroupId = default) => productGroupId != null
            ? GetModifiers().First(x => x.ProductId == id && x.ProductGroupId == productGroupId).Amount
            : GetModifiers().First(x => x.ProductId == id).Amount;
        public IEnumerable<Modifier> SelectedModifiers() => GetModifiers().Where(x => x.Amount != 0);
        public bool HaveModifiers() => GetModifiers().Any();
        public bool IsReachedMaxAmountOfGroupModifier(Guid groupModifierId, Guid modifierId)
        {
            try
            {
                if (groupModifierId == Guid.Empty) return IsReachedMaxAmountOfModifier(modifierId);
                return SimpleGroupModifiers != null && SimpleGroupModifiers.Any()
                    ? SimpleGroupModifiers.First(x => x.Id == groupModifierId).MaxAmount <= 0
                    : throw new Exception($"{typeof(Item).FullName}.{nameof(IsReachedMaxAmountOfGroupModifier)}.{nameof(Exception)}: " +
                        $"No found a group modifier by ID - '{groupModifierId}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(Item).FullName}.{nameof(IsReachedMaxAmountOfGroupModifier)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
                return false;
            }
        }
        public bool IsReachedMinAmountOfGroupModifier(Guid id)
        {
            try
            {
                return SimpleGroupModifiers is not null
                    ? SimpleGroupModifiers.First(x => x.Id == id).MinAmount <= 0
                    : throw new Exception($"{typeof(Item).FullName}.{nameof(IsReachedMinAmountOfGroupModifier)}.{nameof(Exception)}: " +
                        $"No found a group modifier by ID - '{id}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(Item).FullName}.{nameof(IsReachedMinAmountOfGroupModifier)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
                return false;
            }
        }
        public bool IsReachedMaxAmountOfModifier(Guid id)
        {
            try
            {
                return SimpleModifiers is not null
                    ? SimpleModifiers.First(x => x.Id == id).MaxAmount <= 0
                    : throw new Exception($"{typeof(Item).FullName}.{nameof(IsReachedMaxAmountOfModifier)}.{nameof(Exception)}: " +
                        $"No found a modifier by ID - '{id}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(Item).FullName}.{nameof(IsReachedMaxAmountOfModifier)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
                return false;
            }
        }
        public bool IsReachedMinAmountOfModifier(Guid id)
        {
            try
            {
                return SimpleModifiers is not null
                    ? SimpleModifiers.First(x => x.Id == id).MinAmount <= 0
                    : throw new Exception($"{typeof(Item).FullName}.{nameof(IsReachedMinAmountOfModifier)}.{nameof(Exception)}: " +
                        $"No found a modifier by ID - '{id}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(Item).FullName}.{nameof(IsReachedMinAmountOfModifier)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
                return false;
            }
        }
        public bool IsReachedMinAmountOfGroupModifiers()
        {
            if (SimpleGroupModifiers == null || !SimpleGroupModifiers.Any())
                return true;

            bool allow = true;
            foreach (var groupModifier in SimpleGroupModifiers)
                if (groupModifier.MinAmount > 0)
                    return false;
            return allow;
        }
        public bool IsReachedMinAmountOfModifiers()
        {
            if (SimpleModifiers == null || !SimpleModifiers.Any())
                return true;

            bool allow = true;
            foreach (var modifier in SimpleModifiers)
                if (modifier.MinAmount > 0)
                    return false;
            return allow;
        }
        public Item WithSelectedModifiers() => new(ProductId, ProductName ?? string.Empty, Type ?? string.Empty, Amount, Price, ProductSizeId, ComboInformation, PositionId,
            GetModifiers().Where(x => x.Amount != 0), Comment);
        public bool HaveItems() => Amount > 0;
        public double PriceBy() => Price ?? throw new InfoException(typeof(Item).FullName!, nameof(PriceBy),
                nameof(Exception), $"Price of an item by ID - '{ProductId}' can't be null");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public Modifier ModifierById(Guid id)
        {
            var modifiers = Modifiers ?? throw new InfoException(typeof(Item).FullName!,
                nameof(ModifierById), nameof(Exception), $"{nameof(Enumerable)}<{typeof(Modifier).FullName}>", ExceptionType.Null);
            return Modifiers.FirstOrDefault(x => x.ProductId == id) ?? throw new InfoException(typeof(Item).FullName!,
                nameof(ModifierById), nameof(Exception), $"No found a modifier by ID - '{id}'");
        }
        private static void AddSimpleModifier(ref List<Modifier> modifiers, ref List<SimpleModifier> simpleModifiers, TransportModifierItemDto modifier,
            Guid? modifierGroupId = null)
        {
            var modifierName = modifier.Name ?? string.Empty;
            var modifierId = modifier.ItemId ?? throw new InfoException(typeof(Item).FullName!, nameof(AddSimpleModifier),
                nameof(Exception), $"{typeof(TransportModifierItemDto).FullName!}.{nameof(TransportModifierItemDto.ItemId)}", ExceptionType.Null);
            var modifierRestriction = modifier.Restrictions;

            modifiers.Add(new Modifier()
            {
                ProductId = modifierId,
                Name = modifierName,
                MinAmount = modifierRestriction?.MinQuantity,
                MaxAmount = modifierRestriction?.MaxQuantity,
                Amount = modifierRestriction?.ByDefault ?? 0,
                Price = modifier.Price(),
                ProductGroupId = modifierGroupId
            });

            int default_ = modifierRestriction?.ByDefault ?? 0;
            int min = modifierRestriction?.MinQuantity is not null ? (int)modifierRestriction.MinQuantity - default_ : 0;
            int max = modifierRestriction?.MaxQuantity is not null ? (int)modifierRestriction.MaxQuantity - default_ : 0;

            simpleModifiers.Add(new SimpleModifier(modifierId, modifierName, min, max));
        }
        private static void AddSimpleGroupModifier(ref List<SimpleGroupModifier> simpleGroupModifiers, TransportModifierGroupDto groupModifier, int default_)
        {
            var groupModifierName = groupModifier.Name ?? string.Empty;
            var groupModifierId = groupModifier.ItemGroupId ?? throw new InfoException(typeof(Item).FullName!, nameof(AddSimpleGroupModifier),
                nameof(Exception), $"{typeof(TransportModifierGroupDto).FullName!}.{nameof(TransportModifierGroupDto.ItemGroupId)}", ExceptionType.Null);

            int min = groupModifier.Restrictions?.MinQuantity is not null ? (int)groupModifier.Restrictions.MinQuantity - default_ : 0;
            int max = groupModifier.Restrictions?.MaxQuantity is not null ? (int)groupModifier.Restrictions.MaxQuantity - default_ : 0;

            simpleGroupModifiers.Add(new SimpleGroupModifier(groupModifierId, groupModifierName, min, max));
        }
        public object Clone() => new Item(ProductName ?? string.Empty, ProductId, Modifiers, Price, PositionId, Type ?? string.Empty, Amount, ProductSizeId, ComboInformation, Comment,
            SimpleGroupModifiers, SimpleModifiers);
    }
}