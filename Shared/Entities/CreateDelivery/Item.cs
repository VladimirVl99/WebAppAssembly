using WebAppAssembly.Shared.Entities.EMenu;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json.Serialization;
using ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID;
using ApiServerForTelegram.Entities.EExceptions;
using WebAppAssembly.Shared.Entities.IikoCloudApi;
using System.Text.RegularExpressions;

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

            if (product.ItemSizes is not null)
            {
                var simpleModifiers = new List<SimpleModifier>();
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
                                    AddSimpleModifier(ref modifierList, ref simpleModifiers, item);
                                    totalDefault += item.Restrictions?.ByDefault ?? 0;
                                }
                                AddSimpleGroupModifier(ref simpleGroups, modifierGroup, totalDefault);
                            }
                        }    
                    }
                }
                SimpleGroupModifiers = simpleGroups;
                Modifiers = modifierList.Any() ? modifierList : null;
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

        public void IncrementAmount() => Amount++;
        public void DecrementAmount() => Amount = Amount == 0 ? Amount : --Amount;
        public IEnumerable<Modifier> GetModifiers() => Modifiers != null && Modifiers.Any() ? Modifiers : Enumerable.Empty<Modifier>();
        public bool IsSelectedModifier(Guid ModifierId, Guid? productGroupId = default, int? trying = null)
        {
            try
            {
                return productGroupId != null
                    ? (int)GetModifiers().First(x => x.ProductId == ModifierId && x.ProductGroupId == productGroupId).Amount != 0
                    : (int)GetModifiers().First(x => x.ProductId == ModifierId).Amount != 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(Item).FullName}.{nameof(IsSelectedModifier)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
                if (trying != null)
                {
                    if (trying < 50) ++trying;
                    else return false; // ???
                }
                else trying = 0;
                Task.Delay(1);
                return IsSelectedModifier(ModifierId, productGroupId, trying);
            }
        }
        public void IncreaseAmountOfModifier(Guid id, Guid? productGroupId = null)
        {
            if (Modifiers != null && Modifiers.Any())
            {
                if (productGroupId != null && productGroupId != Guid.Empty && SimpleGroupModifiers != null && SimpleGroupModifiers.Any())
                {
                    Modifiers.First(x => x.ProductId == id && x.ProductGroupId == productGroupId).Amount++;
                    var groupModifier = SimpleGroupModifiers.First(x => x.Id == productGroupId);
                    groupModifier.MaxAmount--;
                    groupModifier.MinAmount--;
                }
                else
                {
                    Modifiers.First(x => x.ProductId == id).Amount++;
                    if (SimpleModifiers != null && SimpleModifiers.Any())
                    {
                        var modifier = SimpleModifiers.First(x => x.Id == id);
                        modifier.MaxAmount--;
                        modifier.MinAmount--;
                    }
                }
            }
        }
        public void DecreaseAmountOfModifier(Guid id, Guid? productGroupId = default)
        {
            if (Modifiers != null && Modifiers.Any() && !Modifiers.First(x => x.ProductId == id).Amount.Equals(0))
            {
                if (productGroupId != null && productGroupId != Guid.Empty && SimpleGroupModifiers != null && SimpleGroupModifiers.Any())
                {
                    Modifiers.First(x => x.ProductId == id && x.ProductGroupId == productGroupId).Amount--;
                    var groupModifier = SimpleGroupModifiers.First(x => x.Id == productGroupId);
                    groupModifier.MaxAmount++;
                    groupModifier.MinAmount++;
                }
                else
                {
                    Modifiers.First(x => x.ProductId == id).Amount--;
                    if (SimpleModifiers != null && SimpleModifiers.Any())
                    {
                        var modifier = SimpleModifiers.First(x => x.Id == id);
                        modifier.MaxAmount++;
                        modifier.MinAmount++;
                    }
                }
            }       
            
        }
        public double AmountOfModifier(Guid id, Guid? productGroupId = default) => productGroupId != null
            ? GetModifiers().First(x => x.ProductId == id && x.ProductGroupId == productGroupId).Amount
            : GetModifiers().First(x => x.ProductId == id).Amount;
        public IEnumerable<Modifier> SelectedModifiers() => GetModifiers().Where(x => !x.Amount.Equals(0));
        public bool HaveModifiers() => GetModifiers().Any();
        public bool IsReachedMaxAmountOfGroupModifier(Guid id, int? trying = null)
        {
            try
            {
                return SimpleGroupModifiers != null && SimpleGroupModifiers.Any()
                    ? SimpleGroupModifiers.First(x => x.Id == id).MaxAmount <= 0
                    : throw new Exception($"{typeof(Item).FullName}.{nameof(IsReachedMaxAmountOfGroupModifier)}.{nameof(Exception)}: " +
                        $"No found a group modifier by ID - '{id}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(Item).FullName}.{nameof(IsReachedMaxAmountOfGroupModifier)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
                if (trying != null)
                {
                    if (trying < 50) ++trying;
                    else throw;
                }
                else trying = 0;
                Task.Delay(1);
                return IsReachedMaxAmountOfGroupModifier(id, trying);
            }
        }
        public bool IsReachedMinAmountOfGroupModifier(Guid id, int? trying = null)
        {
            try
            {
                return SimpleGroupModifiers != null && SimpleGroupModifiers.Any()
                    ? SimpleGroupModifiers.First(x => x.Id == id).MinAmount <= 0
                    : throw new Exception($"{typeof(Item).FullName}.{nameof(IsReachedMinAmountOfGroupModifier)}.{nameof(Exception)}: " +
                        $"No found a group modifier by ID - '{id}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(Item).FullName}.{nameof(IsReachedMinAmountOfGroupModifier)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
                if (trying != null)
                {
                    if (trying < 50) ++trying;
                    else throw;
                }
                else trying = 0;
                Task.Delay(1);
                return IsReachedMinAmountOfGroupModifier(id, trying);
            }
        }
        public bool IsReachedMaxAmountOfModifier(Guid id, int? trying = null)
        {
            try
            {
                return SimpleModifiers != null && SimpleModifiers.Any()
                    ? SimpleModifiers.First(x => x.Id == id).MaxAmount <= 0
                    : throw new Exception($"{typeof(Item).FullName}.{nameof(IsReachedMaxAmountOfModifier)}.{nameof(Exception)}: " +
                        $"No found a modifier by ID - '{id}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(Item).FullName}.{nameof(IsReachedMaxAmountOfModifier)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
                if (trying != null)
                {
                    if (trying < 50) ++trying;
                    else throw;
                }
                else trying = 0;
                Task.Delay(1);
                return IsReachedMaxAmountOfModifier(id, trying);
            }
        }
        public bool IsReachedMinAmountOfModifier(Guid id, int? trying = null)
        {
            try
            {
                return SimpleModifiers != null && SimpleModifiers.Any()
                    ? SimpleModifiers.First(x => x.Id == id).MinAmount <= 0
                    : throw new Exception($"{typeof(Item).FullName}.{nameof(IsReachedMinAmountOfModifier)}.{nameof(Exception)}: " +
                        $"No found a modifier by ID - '{id}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(Item).FullName}.{nameof(IsReachedMinAmountOfModifier)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
                if (trying != null)
                {
                    if (trying < 50) ++trying;
                    else throw;
                }
                else trying = 0;
                Task.Delay(1);
                return IsReachedMinAmountOfModifier(id, trying);
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
        private static void AddSimpleModifier(ref List<Modifier> modifiers, ref List<SimpleModifier> simpleModifiers, TransportModifierItemDto modifier)
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
                Price = modifier.Price()
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