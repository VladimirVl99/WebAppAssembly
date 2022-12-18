using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Entities.WebApp
{
    public enum PageViewType
    {
        SelectingProducts,
        SelectingModifiersAndAmountsForProduct,
        SelectingAmountsForProducts,
        ChangingSelectedProductsWithModifiers,
        ShoppingCart,
        InfoAboutCreatedOrder
    }
}
