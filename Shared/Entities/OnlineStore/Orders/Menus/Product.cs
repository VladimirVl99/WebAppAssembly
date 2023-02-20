using WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus;
using WebAppAssembly.Shared.Entities.Exceptions;
using ProductRequest = WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus.Product;

namespace WebAppAssembly.Shared.Entities.OnlineStore.Orders.Menus
{
    /// <summary>
    /// Information about a product.
    /// </summary>
    public class Product : ProductRequest, IProduct
    {
        public bool HaveModifiers
            => Sizes.FirstOrDefault()?.ModifierGroups?.Any() ?? false;

        public bool HaveSeveralSizes
        {
            get
            {
                if (Sizes is null) return false;
                int i = 0;
                foreach (var size in Sizes)
                {
                    if (i > 0) return true;
                    i++;
                }
                return false;
            }
        }

        public bool HaveModifiersOrSeveralSizes
            => HaveModifiers || HaveSeveralSizes;


        public string ImageLink()
            => Sizes.FirstOrDefault()?.ButtonImageUrl ?? string.Empty;

        public float? PriceOrNull(Guid? sizeId = null)
            => sizeId is null
            ? Sizes.FirstOrDefault()?.Prices?.FirstOrDefault()?.Amount
            : Sizes.FirstOrDefault(x => x.SizeId == sizeId)?.Prices?.FirstOrDefault()?.Amount;

        public float Price(Guid? sizeId = null)
            => PriceOrNull(sizeId) ?? throw new InfoException(typeof(Product).FullName!,
            nameof(Price), nameof(Exception), $"Price of size ID - '{(sizeId is null ? "default ID" : sizeId)}' is null");

        public float Weight()
            => Sizes.FirstOrDefault()?.PortionWeightGrams ?? 0;

        public float Fats()
            => Sizes.FirstOrDefault()?.NutritionPerHundredGrams?.Fats ?? 0;

        public float Proteins()
            => Sizes.FirstOrDefault()?.NutritionPerHundredGrams?.Proteins ?? 0;

        public float Carbs()
            => Sizes.FirstOrDefault()?.NutritionPerHundredGrams?.Carbs ?? 0;

        public float Energy()
            => Sizes.FirstOrDefault()?.NutritionPerHundredGrams?.Energy ?? 0;

        public IEnumerable<GroupModifier>? ModifierGroups()
            => Sizes.FirstOrDefault()?.ModifierGroups;
    }
}
