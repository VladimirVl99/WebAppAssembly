using Microsoft.JSInterop;
using WebAppAssembly.Client.Entities;

namespace WebAppAssembly.Client.Repositories.JsHelper
{
    public class JsHelperService : IJsHelperService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ScrollToTopAsync(IJSRuntime jsRuntime) => await jsRuntime.InvokeVoidAsync(JsHelperMethodNames.ScrollToTop.ToString());
    }
}
