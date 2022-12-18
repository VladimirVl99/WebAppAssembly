namespace WebAppAssembly.Shared.Entities.WebApp
{
    public class StateOfPage
    {
        /// <summary>
        /// 
        /// </summary>
        public StateOfPage()
        {
            PageTypeOfOrder = PageViewType.SelectingProducts;
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
        public StateOfPage(PageViewType pageTypeOfOrder, bool renderComplete, bool haveErrors, bool isPageBlocked)
        {
            PageTypeOfOrder = pageTypeOfOrder;
            RenderComplete = renderComplete;
            HaveErrors = haveErrors;
            IsPageBlocked = isPageBlocked;
        }

        public PageViewType PageTypeOfOrder { get; set; }
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
        public void GoToPage(PageViewType pageType) => PageTypeOfOrder = pageType;

        /// <summary>
        /// 
        /// </summary>
        public void GoBack()
        {
            PageTypeOfOrder = PageTypeOfOrder switch
            {
                PageViewType.SelectingAmountsForProducts => PageViewType.SelectingProducts,
                PageViewType.SelectingModifiersAndAmountsForProduct => PageViewType.SelectingProducts,
                PageViewType.ChangingSelectedProductsWithModifiers => PageViewType.SelectingProducts,
                PageViewType.ShoppingCart => PageViewType.SelectingProducts,
                PageViewType.InfoAboutCreatedOrder => PageViewType.ShoppingCart,
                _ => PageViewType.SelectingProducts
            };
        }
    }
}
