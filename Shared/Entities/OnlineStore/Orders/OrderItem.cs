using System.Data;
using WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders;
using Modifier = WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders.Modifier;
using WebAppAssembly.Shared.Entities.Exceptions;
using SimpleModifierRequest = WebAppAssembly.Shared.Entities.Api.Requests.Delivery.Orders.SimpleModifier;
using SimpleGroupModifierRequest = WebAppAssembly.Shared.Entities.Api.Requests.Delivery.Orders.SimpleGroupModifier;
using WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus;
using Product = WebAppAssembly.Shared.Entities.OnlineStore.Orders.Menus.Product;

namespace WebAppAssembly.Shared.Entities.OnlineStore.Orders
{
    /// <summary>
    /// Information about an order item (product).
    /// </summary>
    public class OrderItem : IOrderItemProcessing
    {
        #region Fields

        private const string DefaultItemName = "???";

        private double _price;
        private double _totalPrice;
        private double _totalPriceOfModifiers;
        private string _productName = string.Empty;
        private IEnumerable<Modifier>? _modifiers;

        #endregion

        #region Properties

        public string ProductName
        {
            get => _productName;
            private set => _productName = value ?? string.Empty;
        }

        public Guid ProductId { get; }

        public IEnumerable<Modifier> Modifiers
        {
            get => _modifiers ?? Enumerable.Empty<Modifier>();
            private set => _modifiers = value;
        }

        public double Price
        {
            get => _price;
            private set => _price = value;
        }

        public double PriceWithSelectedModifiers
            => _price + _totalPriceOfModifiers;

        public double TotalPrice
        {
            get => _totalPrice;
            private set => _totalPrice = value;
        }

        public double TotalPriceOfModifiers
        {
            get => _totalPriceOfModifiers;
            private set => _totalPriceOfModifiers = value;
        }

        public Guid? PositionId { get; private set; }

        public OrderItemType Type { get; private set; }

        public double Amount { get; private set; }

        public Guid? ProductSizeId { get; private set; }

        public ComboInformation? ComboInformation { get; private set; }

        public string? Comment { get; private set; }

        public IEnumerable<SimpleGroupModifier>? SimpleGroupModifiers { get; }

        public IEnumerable<SimpleModifier>? SimpleModifiers { get; }

        public bool HaveModifiers
            => Modifiers.Any();

        public bool HaveItems
            => Amount > 0;

        #endregion

        #region Constructors

        public OrderItem(Guid productId, string productName, IEnumerable<Modifier>? modifiers, double price,
            double totalPrice, double totalPriceOfModifiers, Guid? positionId, OrderItemType type,
            double amount, Guid? productSizeId, ComboInformation? comboInformation, string? comment,
            IEnumerable<SimpleGroupModifier>? simpleGroupModifiers, IEnumerable<SimpleModifier>? simpleModifiers)
        {
            ProductId = productId;
            ProductName = productName;
            Type = type;
            Amount = amount;
            _totalPrice = totalPrice;
            ProductSizeId = productSizeId;
            ComboInformation = comboInformation;
            PositionId = positionId;
            _modifiers = modifiers;
            Comment = comment;
            _price = price;
            _totalPriceOfModifiers = totalPriceOfModifiers;
            SimpleModifiers = simpleModifiers;
            SimpleGroupModifiers = simpleGroupModifiers;
        }

        /// <summary>
        /// Must be used for modifiable items.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="positionId"></param>
        /// <exception cref="InfoException"></exception>
        public OrderItem(Product product, Guid positionId, Guid? sizeId = null)
        {
            ProductId = product.Id;
            ProductName = product.Name;
            Type = product.OrderItemType;
            PositionId = positionId;
            _price = product.PriceOrNull() ?? default;
            _totalPrice = 0;
            _totalPriceOfModifiers = 0;
            ProductSizeId = sizeId;

            var modifiers = new List<Modifier>();
            var simpleGroups = new List<SimpleGroupModifier>();
            var simpleModifiers = new List<SimpleModifier>();


            foreach (var size in product.Sizes)
            {
                if (size.ModifierGroups is not null)
                {
                    foreach (var modifierGroup in size.ModifierGroups)
                    {
                        // The amount of selected items by default
                        int totalDefault = 0;

                        foreach (var item in modifierGroup.Items)
                        {
                            AddSimpleModifier(
                                modifiers,
                                simpleModifiers,
                                modifier: item,
                                modifierGroupId: modifierGroup.ItemGroupId);

                            totalDefault += item.Restrictions?.ByDefault ?? 0;
                        }

                        AddSimpleGroupModifier(simpleGroups, modifierGroup, totalDefault);
                    }
                }
            }

            SimpleGroupModifiers = simpleGroups;
            _modifiers = modifiers.Any() ? modifiers : null;
            SimpleModifiers = simpleModifiers;
        }

        public OrderItem(Guid productId, string productName, double amount, OrderItemType type)
        {
            ProductId = productId;
            ProductName = productName;
            Amount = amount;
            Type = type;
        }

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// Increments the item's quantity and increases the total price.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static OrderItem operator ++(OrderItem item)
        {
            item.Amount++;
            item.IncreaseTotalPrice();
            return item;
        }

        /// <summary>
        /// Decrements the item's quantity and decreases the total price.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static OrderItem operator --(OrderItem item)
        {
            if (item.Amount <= 0) return item;
            --item.Amount;
            item.DecreaseTotalPrice();
            return item;
        }

        public double IncreaseQuantityAndPrice()
        {
            Amount++;
            return IncreaseTotalPrice();
        }

        public double DecrementQuantityWithPrice()
        {
            if (Amount <= 0) return 0;
            --Amount;
            return DecreaseTotalPrice();
        }

        public bool AreModifierSelected(Guid modifierId, Guid? modifierGroupId = null)
        {
            try
            {
                return modifierGroupId is not null && modifierGroupId != Guid.Empty
                    ? (int)Modifiers.First(x => x.ProductId == modifierId && x.ProductGroupId == modifierGroupId).Amount > 0
                    : (int)Modifiers.First(x => x.ProductId == modifierId).Amount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(OrderItem).FullName}.{nameof(AreModifierSelected)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
                return false;
            }
        }

        public (double changedPriceBy, bool isMaxQuantityReached) IncreaseQuantityWithPriceOfModifier(Guid modifierId,
            Guid? modifierGroupId = null)
        {
            const string defaultException = "'{0}.{1}' cannot be less than zero. HIGH ATTENTION!";

            var modifier = ModifierById(modifierId);

            // The maximum and minimum number of modifier's groups decreases, while the limit for
            // the maximum number is zero
            if (modifierGroupId is not null && modifierGroupId != Guid.Empty && SimpleGroupModifiers is not null)
            {
                var groupModifier = SimpleGroupModifierById(SimpleGroupModifiers, (Guid)modifierGroupId);

                if (groupModifier.MaxQuantity < 0)
                {
                    throw new InfoException(typeof(OrderItem).FullName!, nameof(IncreaseQuantityWithPriceOfModifier), nameof(Exception),
                        string.Format(defaultException, typeof(SimpleGroupModifier).FullName!, nameof(SimpleGroupModifier.MaxQuantity)));
                }
                else if ((int)groupModifier.MaxQuantity == 0)
                {
                    return (0, true);
                }

                groupModifier.MaxQuantity--;
                groupModifier.MinQuantity--;
            }
            // The maximum and minimum number of modifiers decreases, while the limit for
            // the maximum number is zero
            else if (SimpleModifiers is not null)
            {
                var simpleModifier = SimpleModifierById(SimpleModifiers, modifierId);

                if (simpleModifier.MaxQuantity < 0)
                {
                    throw new InfoException(typeof(OrderItem).FullName!, nameof(IncreaseQuantityWithPriceOfModifier), nameof(Exception),
                        string.Format(defaultException, typeof(SimpleModifier).FullName!, nameof(SimpleModifier.MaxQuantity)));
                }
                else if ((int)simpleModifier.MaxQuantity == 0)
                {
                    return (0, true);
                }

                simpleModifier.MaxQuantity--;
                simpleModifier.MinQuantity--;
            }

            modifier.Amount++;
            return (IncreaseTotalPrice(modifier), false);
        }

        public (double changedPriceBy, bool isMinQuantityReached) DecreaseQuantityWithPriceOfModifier(Guid modifierId,
            Guid? modifierGroupId = null)
        {
            const string defaultException = "'{0}.{1}' cannot be more than zero. HIGH ATTENTION!";

            var modifier = ModifierById(modifierId);

            if ((int)modifier.Amount <= 0)
            {
                return (0, false);
            }
            // The maximum and minimum number of modifier's groups increases, while the limit for
            // the minimum number is zero
            else if (modifierGroupId is not null && modifierGroupId != Guid.Empty && SimpleGroupModifiers is not null)
            {
                var groupModifier = SimpleGroupModifierById(SimpleGroupModifiers, (Guid)modifierGroupId);

                if (groupModifier.MinQuantity > 0)
                {
                    throw new InfoException(typeof(OrderItem).FullName!, nameof(DecreaseQuantityWithPriceOfModifier), nameof(Exception),
                        string.Format(defaultException, typeof(SimpleGroupModifier).FullName!, nameof(SimpleGroupModifier.MinQuantity)));
                }
                else if ((int)groupModifier.MinQuantity == 0)
                {
                    return (0, true);
                }

                groupModifier.MaxQuantity++;
                groupModifier.MinQuantity++;
            }
            // The maximum and minimum number of modifier's groups increases, while the limit for
            // the minimum number is zero
            else if (SimpleModifiers is not null)
            {
                var simpleModifier = SimpleModifierById(SimpleModifiers, modifierId);

                if (simpleModifier.MinQuantity > 0)
                {
                    throw new InfoException(typeof(OrderItem).FullName!, nameof(DecreaseQuantityWithPriceOfModifier), nameof(Exception),
                        string.Format(defaultException, typeof(SimpleModifier).FullName!, nameof(SimpleModifier.MinQuantity)));
                }
                else if ((int)simpleModifier.MinQuantity == 0)
                {
                    return (0, true);
                }

                simpleModifier.MaxQuantity++;
                simpleModifier.MinQuantity++;
            }

            modifier.Amount--;
            return (DecreaseTotalPrice(modifier), false);
        }

        public double AmountOfModifier(Guid id, Guid? productGroupId = null) => productGroupId is not null
            ? Modifiers.First(x => x.ProductId == id && x.ProductGroupId == productGroupId).Amount
            : Modifiers.First(x => x.ProductId == id).Amount;

        public IEnumerable<Modifier> SelectedModifiers()
            => Modifiers.Where(x => x.Amount > 0);

        public bool IsMaxAmountOfGroupModifierReached(Guid groupModifierId, Guid modifierId)
        {
            try
            {
                if (groupModifierId == Guid.Empty) return IsMaxAmountOfModifierReached(modifierId);
                return SimpleGroupModifiers is not null && SimpleGroupModifiers.Any()
                    ? SimpleGroupModifiers.First(x => x.Id == groupModifierId).MaxQuantity <= 0
                    : throw new Exception($"{typeof(OrderItem).FullName}.{nameof(IsMaxAmountOfGroupModifierReached)}.{nameof(Exception)}: " +
                        $"No found a group modifier by ID - '{groupModifierId}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(OrderItem).FullName}.{nameof(IsMaxAmountOfGroupModifierReached)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
                return false;
            }
        }

        public bool IsMinAmountOfGroupModifierReached(Guid id)
        {
            try
            {
                return SimpleGroupModifiers is not null
                    ? SimpleGroupModifiers.First(x => x.Id == id).MinQuantity <= 0
                    : throw new Exception($"{typeof(OrderItem).FullName}.{nameof(IsMinAmountOfGroupModifierReached)}.{nameof(Exception)}: " +
                        $"No found a group modifier by ID - '{id}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(OrderItem).FullName}.{nameof(IsMinAmountOfGroupModifierReached)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
                return false;
            }
        }

        public bool IsMaxAmountOfModifierReached(Guid id)
        {
            try
            {
                return SimpleModifiers is not null
                    ? SimpleModifiers.First(x => x.Id == id).MaxQuantity <= 0
                    : throw new Exception($"{typeof(OrderItem).FullName}.{nameof(IsMaxAmountOfModifierReached)}.{nameof(Exception)}: " +
                        $"No found a modifier by ID - '{id}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(OrderItem).FullName}.{nameof(IsMaxAmountOfModifierReached)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
                return false;
            }
        }

        public bool IsMinAmountOfModifierReached(Guid id)
        {
            try
            {
                return SimpleModifiers is not null
                    ? SimpleModifiers.First(x => x.Id == id).MinQuantity <= 0
                    : throw new Exception($"{typeof(OrderItem).FullName}.{nameof(IsMinAmountOfModifierReached)}.{nameof(Exception)}: " +
                        $"No found a modifier by ID - '{id}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{typeof(OrderItem).FullName}.{nameof(IsMinAmountOfModifierReached)}.{nameof(Exception)}: " +
                    $"{ex.Message}");
                return false;
            }
        }

        public bool IsMinAmountOfGroupModifiersReached()
        {
            if (SimpleGroupModifiers is null)
                return true;

            bool allow = true;

            foreach (var groupModifier in SimpleGroupModifiers)
            {
                if (groupModifier.MinQuantity > 0)
                {
                    return false;
                }
            }

            return allow;
        }

        public bool IsMinAmountOfModifiersReached()
        {
            if (SimpleModifiers is null)
                return true;

            bool allow = true;

            foreach (var modifier in SimpleModifiers)
            {
                if (modifier.MinQuantity > 0)
                {
                    return false;
                }
            }

            return allow;
        }

        public double ChangeSize(Guid sizeId, float newPrice)
        {
            ProductSizeId = sizeId;
            var different = Amount * (newPrice - _price);
            _totalPrice += different;
            _price = newPrice;
            return different;
        }

        #endregion

        #region Private

        /// <summary>
        /// Increases the total price of this position.
        /// Also, the total price of the selected modifiers is taken into the calculation
        /// of the price.
        /// Returns the amount by which the price was changed.
        /// </summary>
        /// <returns></returns>
        private double IncreaseTotalPrice()
        {
            double priceOfChanged = PriceWithSelectedModifiers;
            _totalPrice += priceOfChanged;
            return priceOfChanged;
        }

        /// <summary>
        /// Decreases the total price of this position.
        /// Also, the total price of the selected modifiers is taken into the calculation
        /// of the price.
        /// Returns the amount by which the price was changed.
        /// </summary>
        /// <returns></returns>
        private double DecreaseTotalPrice()
        {
            var priceOfChanged = PriceWithSelectedModifiers;
            _totalPrice -= (int)Amount == 0 ? 0 : priceOfChanged;
            return priceOfChanged;
        }

        /// <summary>
        /// Increases the total price of this position.
        /// Formula for the total price of the selected modifiers: the total price of selected modifiers + modifier's price.
        /// Formula for the total price of this position: the total price of this product position + ( modifier's price * amount of this position ).
        /// Returns the amount by which the price was changed.
        /// </summary>
        /// <param name="modifier"></param>
        /// <returns></returns>
        private double IncreaseTotalPrice(Modifier modifier)
        {
            var price = modifier.Price;
            // Increases the total price of all selected modifiers
            _totalPriceOfModifiers += price;
            // The value by which the total price should increase depending on the selected quantity of this item
            // and the amount of the added modifier
            var increaseBy = price * Amount;
            _totalPrice += increaseBy;
            return increaseBy;
        }

        /// <summary>
        /// Decreases the total price of this position.
        /// Formula for the total price of the selected modifiers: the total price of selected modifiers - modifier's price.
        /// Formula for the total price of this position: the total price of this product position - ( modifier's price * amount of this position ).
        /// Returns the amount by which the price was changed.
        /// </summary>
        /// <param name="modifier"></param>
        /// <returns></returns>
        private double DecreaseTotalPrice(Modifier modifier)
        {
            var price = modifier.Price;
            // Decreases the total price of all selected modifiers
            _totalPriceOfModifiers -= price;
            // The value by which the total price should decrease depending on the selected quantity of this item
            // and the amount of the added modifier
            var increaseBy = price * Amount;
            _totalPrice -= increaseBy;
            return increaseBy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simpleGroups"></param>
        /// <param name="modifierGroupId"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        private static SimpleGroupModifier SimpleGroupModifierById(IEnumerable<SimpleGroupModifier> simpleGroups, Guid modifierGroupId)
            => simpleGroups.FirstOrDefault(x => x.Id == modifierGroupId) ?? throw new InfoException(typeof(OrderItem).FullName!,
                nameof(SimpleGroupModifierById), nameof(Exception), $"No found a modifier's group by ID - '{modifierGroupId}'");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simpleModifiers"></param>
        /// <param name="modifierId"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        private static SimpleModifier SimpleModifierById(IEnumerable<SimpleModifier> simpleModifiers, Guid modifierId)
            => simpleModifiers.FirstOrDefault(x => x.Id == modifierId) ?? throw new InfoException(typeof(OrderItem).FullName!,
                nameof(SimpleModifierById), nameof(Exception), $"No found a modifier by ID - '{modifierId}'");

        /// <summary>
        /// Returns modifier by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        private Modifier ModifierById(Guid id)
            => Modifiers.FirstOrDefault(x => x.ProductId == id) ?? throw new InfoException(typeof(OrderItem).FullName!,
                nameof(ModifierById), nameof(Exception), $"No found a modifier by ID - '{id}'");

        /// <summary>
        /// Adding modifiers to this item.
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="simpleModifiers"></param>
        /// <param name="modifier"></param>
        /// <param name="modifierGroupId"></param>
        /// <exception cref="InfoException"></exception>
        private void AddSimpleModifier(List<Modifier> modifiers, List<SimpleModifier> simpleModifiers, Api.Common.IikoTransport.ExternalMenus.Modifier modifier,
            Guid? modifierGroupId = null)
        {
            var modifierName = string.IsNullOrWhiteSpace(modifier.Name) ? DefaultItemName : modifier.Name;

            var modifierId = modifier.Id;

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

            int _default = modifierRestriction?.ByDefault ?? 0;
            if (_default > 0) _totalPriceOfModifiers += _default * modifier.Price();
            int min = modifierRestriction?.MinQuantity is not null ? modifierRestriction.MinQuantity - _default : 0;
            int max = modifierRestriction?.MaxQuantity is not null ? modifierRestriction.MaxQuantity - _default : 0;

            simpleModifiers.Add(new SimpleModifierRequest(modifierId, modifierName, min, max));
        }

        /// <summary>
        /// Adding modifier's groups to this item.
        /// </summary>
        /// <param name="simpleGroupModifiers"></param>
        /// <param name="groupModifier"></param>
        /// <param name="default_"></param>
        /// <exception cref="InfoException"></exception>
        private static void AddSimpleGroupModifier(List<SimpleGroupModifier> simpleGroupModifiers, GroupModifier groupModifier,
            int default_)
        {
            var groupModifierName = groupModifier.Name;

            var groupModifierId = groupModifier.ItemGroupId;

            int min = groupModifier.Restrictions?.MinQuantity is not null ? groupModifier.Restrictions.MinQuantity - default_ : 0;
            int max = groupModifier.Restrictions?.MaxQuantity is not null ? groupModifier.Restrictions.MaxQuantity - default_ : 0;

            simpleGroupModifiers.Add(new SimpleGroupModifierRequest(groupModifierId, groupModifierName, min, max));
        }

        #endregion

        #endregion
    }
}