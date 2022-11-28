using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Entities.WebApp
{
    public class StateOfPage
    {
        /// <summary>
        /// 
        /// </summary>
        public StateOfPage()
        {
            PageTypeOfOrder = PageTypeOfOrder.SelectingProducts;
            RenderComplete = false;
            HaveErrors = false;
            IsPageBlocked = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageTypeOfOrder"></param>
        /// <param name="renderComplete"></param>
        /// <param name="haveErrors"></param>
        /// <param name="isPageBlocked"></param>
        public StateOfPage(PageTypeOfOrder pageTypeOfOrder, bool renderComplete, bool haveErrors, bool isPageBlocked)
        {
            PageTypeOfOrder = pageTypeOfOrder;
            RenderComplete = renderComplete;
            HaveErrors = haveErrors;
            IsPageBlocked = isPageBlocked;
        }

        public PageTypeOfOrder PageTypeOfOrder { get; set; }
        public bool RenderComplete { get; set; }
        public bool HaveErrors { get; set; }
        public bool IsPageBlocked { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public void SetErrorStateForWebApp() => HaveErrors = true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageType"></param>
        public void GoToPage(PageTypeOfOrder pageType) => PageTypeOfOrder = pageType;

        /// <summary>
        /// 
        /// </summary>
        public void GoBack()
        {
            PageTypeOfOrder = PageTypeOfOrder switch
            {
                PageTypeOfOrder.SelectingAmountsForProducts => PageTypeOfOrder.SelectingProducts,
                PageTypeOfOrder.SelectingModifiersAndAmountsForProduct => PageTypeOfOrder.SelectingProducts,
                PageTypeOfOrder.ChangingSelectedProductsWithModifiers => PageTypeOfOrder.SelectingProducts,
                PageTypeOfOrder.ShoppingCart => PageTypeOfOrder.SelectingProducts,
                PageTypeOfOrder.InfoAboutCreatedOrder => PageTypeOfOrder.ShoppingCart,
                _ => PageTypeOfOrder.SelectingProducts
            };
        }
    }
}
