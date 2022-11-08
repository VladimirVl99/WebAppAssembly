using WebAppAssembly.Shared.Entities.EMenu;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json.Serialization;


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

        public Item(Guid productId, string productName, Guid? positionId = default, IEnumerable<EMenu.Modifier>? modifiers = null,
            IEnumerable<GroupModifier>? groupModifiers = default, double? price = null, IEnumerable<Product>? products = null,
            IEnumerable<Group>? groups = null)
        {
            ProductId = productId;
            ProductName = productName;
            Type = "Product";
            PositionId = positionId != default && positionId != Guid.Empty ? positionId : Guid.NewGuid();
            Price = price;

            var modifierList = new List<Modifier>();
            if (modifiers != null && modifiers.Any())
            {
                var simpleModifiers = new List<SimpleModifier>();
                foreach (var modifier in modifiers) AddSimpleModifier(ref modifierList, ref simpleModifiers, modifier, products);
                SimpleModifiers = simpleModifiers;
            }
            if(groupModifiers != null && groupModifiers.Any())
            {
                var simpleGroups = new List<SimpleGroupModifier>();
                foreach (var groupModifier in groupModifiers)
                {
                    int totalDefaultAmount = 0;
                    if (groupModifier.ChildModifiers is not null && groupModifier.ChildModifiers.Any())
                    {
                        foreach (var childModifier in groupModifier.ChildModifiers)
                        {
                            int defaultAmount = childModifier.DefaultAmount != null ? (int)childModifier.DefaultAmount : 0;
                            totalDefaultAmount += defaultAmount;
                            string modifierName = products?.FirstOrDefault(x => x.Id == childModifier.Id)?.Name ?? string.Empty;
                            modifierList.Add(new Modifier()
                            {
                                ProductId = childModifier.Id,
                                Name = modifierName,
                                ProductGroupId = groupModifier.Id,
                                MinAmount = childModifier.MinAmount,
                                MaxAmount = childModifier.MaxAmount,
                                Amount = defaultAmount,
                                Price = products?.First(x => x.Id == childModifier.Id).Price()
                            });
                        }
                    }
                    string name = groups?.FirstOrDefault(x => x.Id == groupModifier.Id)?.Name ?? string.Empty;
                    simpleGroups.Add(new SimpleGroupModifier(groupModifier.Id, name, groupModifier.MinAmount - totalDefaultAmount,
                        groupModifier.MaxAmount - totalDefaultAmount));
                }
                SimpleGroupModifiers = simpleGroups;
            }
            Modifiers = modifierList.Count != 0 ? modifierList : null;
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
        private static void AddSimpleModifier(ref List<Modifier> modifiers, ref List<SimpleModifier> simpleModifiers, EMenu.Modifier modifier,
            IEnumerable<Product>? products = null)
        {
            int defaultAmount = modifier.DefaultAmount != null ? (int)modifier.DefaultAmount : 0;
            string name = products?.FirstOrDefault(x => x.Id == modifier.Id)?.Name ?? string.Empty;
            modifiers.Add(new Modifier()
            {
                ProductId = modifier.Id,
                Name = name,
                MinAmount = modifier.MinAmount,
                MaxAmount = modifier.MaxAmount,
                Amount = defaultAmount,
                Price = products?.First(x => x.Id == modifier.Id).Price()
            });
            simpleModifiers.Add(new SimpleModifier(modifier.Id, name, modifier.MinAmount - defaultAmount, modifier.MaxAmount - defaultAmount));
        }
        public object Clone() => new Item(ProductName ?? string.Empty, ProductId, Modifiers, Price, PositionId, Type ?? string.Empty, Amount, ProductSizeId, ComboInformation, Comment,
            SimpleGroupModifiers, SimpleModifiers);
    }
}