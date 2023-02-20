using Microsoft.JSInterop;

namespace WebAppAssembly.Client.Repositories.JsHelper
{
    /// <summary>
    /// Auxiliary class for working with JS code.
    /// </summary>
    public class JsHelperService : IJsHelperService
    {
        private const string DefaultScrollToTop = "ScrollToTop";


        public async Task ScrollToTopAsync(IJSRuntime jsRuntime) => await jsRuntime.InvokeVoidAsync(DefaultScrollToTop);
    }
}
