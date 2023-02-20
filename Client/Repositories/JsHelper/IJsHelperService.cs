using Microsoft.JSInterop;

namespace WebAppAssembly.Client.Repositories.JsHelper
{
    /// <summary>
    /// Auxiliary interface for working with JS code.
    /// </summary>
    public interface IJsHelperService
    {
        /// <summary>
        /// Scroll the page to top.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <returns></returns>
        Task ScrollToTopAsync(IJSRuntime jsRuntime);
    }
}
