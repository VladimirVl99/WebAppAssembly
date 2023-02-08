using WebAppAssembly.Shared.Entities.WebAppPage;

namespace WebAppAssembly.Shared.Entities.WebApp
{
    /// <summary>
    /// The state of web application page.
    /// </summary>
    public class PageState
    {
        #region Properties

        /// <summary>
        /// Displayed page view.
        /// </summary>
        public PageViewType PageTypeOfOrder { get; set; }

        /// <summary>
        /// Render complete flag.
        /// </summary>
        public bool RenderComplete { get; set; }

        /// <summary>
        /// Error state of the web application.
        /// </summary>
        public bool HaveErrors { get; private set; }

        /// <summary>
        /// The page is blocked for any activities.
        /// </summary>
        public bool IsPageBlocked { get; set; }

        #endregion

        #region Constructors

        public PageState()
        {
            PageTypeOfOrder = PageViewType.SelectingProducts;
            RenderComplete = false;
            HaveErrors = false;
            IsPageBlocked = false;
        }

        public PageState(PageViewType pageTypeOfOrder, bool renderComplete, bool haveErrors, bool isPageBlocked)
        {
            PageTypeOfOrder = pageTypeOfOrder;
            RenderComplete = renderComplete;
            HaveErrors = haveErrors;
            IsPageBlocked = isPageBlocked;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets error status for the web application.
        /// </summary>
        public void SetErrorStateForWebApp()
            => HaveErrors = true;

        /// <summary>
        /// Goes to the specified page.
        /// </summary>
        /// <param name="pageType"></param>
        public void GoToPage(PageViewType pageType)
            => PageTypeOfOrder = pageType;

        /// <summary>
        /// Goes back to the previous page.
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

        #endregion
    }
}
