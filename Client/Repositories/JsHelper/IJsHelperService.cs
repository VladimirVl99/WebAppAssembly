using Microsoft.JSInterop;

namespace WebAppAssembly.Client.Repositories.JsHelper
{
    public interface IJsHelperService
    {
        Task ScrollToTopAsync(IJSRuntime jsRuntime);
    }
}
