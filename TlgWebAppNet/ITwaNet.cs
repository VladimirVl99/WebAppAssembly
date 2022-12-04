using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppAssembly.Shared.Entities.Telegram;

namespace TlgWebAppNet
{
    public interface ITwaNet
    {
        long ChatId { get; }

        Task<long> TwaNetInitAsync(IJSRuntime jsRuntime);
        Task<long> TwaNetInitAsync(IJSRuntime jsRuntime, string btnColor);
        Task SetMainButtonTextAsync(IJSRuntime jsRuntime, string txt);
        Task SetHapticFeedbackSelectionChangedAsync(IJSRuntime jsRuntime);
        Task ShowBackButtonAsync(IJSRuntime jsRuntime);
        Task HideBackButtonAsync(IJSRuntime jsRuntime);
        Task HideMainButtonAsync(IJSRuntime jsRuntime);
        Task ShowOkPopupMessageAsync(IJSRuntime jsRuntime, string title, string description, HapticFeedBackNotificationType notificationType);
        Task ShowProgressAsync(IJSRuntime jsRuntime);
        Task HideProgressAsync(IJSRuntime jsRuntime);
        Task CloseWebAppAsync(IJSRuntime jsRuntime);
        Task ListenMainButtonAsync(IJSRuntime jsRuntime);
        Task ListenBackButtonAsync(IJSRuntime jsRuntime);
        Task SetMainBtnColorAsync(IJSRuntime jsRuntime, string color);
        Task<InvoiceClosedStatus> InvoiceClosedHandlerAsync(IJSRuntime jsRuntime, string invoiceLink);
        Task SetHapticFeedbackImpactOccurredAsync(IJSRuntime jsRuntime, HapticFeedbackImpactOccurredType style);
        Task SetHapticFeedbackNotificationAsync(IJSRuntime jsRuntime, HapticFeedBackNotificationType type);
        Task<string> ShowPopupParamsAsync(IJSRuntime jsRuntime, PopupParams popup, HapticFeedBackNotificationType? type = null,
            HapticFeedbackImpactOccurredType? style = null);
    }
}
