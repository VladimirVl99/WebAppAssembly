using Microsoft.JSInterop;
using WebAppAssembly.Client.Entities;

namespace WebAppAssembly.Client.Repositories.JsHelper
{
    public class JsHelperService : IJsHelperService
    {
        public JsHelperService(IJSRuntime jsRuntime) => JsRuntime = jsRuntime;

        private readonly IJSRuntime JsRuntime;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ScrollToTopAsync() => await JsRuntime.InvokeVoidAsync(JsHelperMethodNames.ScrollToTop.ToString());
    }
}
