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

        Task<long> TwaNetInitAsync();
        Task<long> TwaNetInitAsync(string btnColor);
        Task SetMainButtonTextAsync(string txt);
        Task SetHapticFeedbackSelectionChangedAsync();
        Task ShowBackButtonAsync();
        Task HideBackButtonAsync();
        Task HideMainButtonAsync();
        Task ShowOkPopupMessageAsync(string title, string description, HapticFeedBackNotificationType notificationType);
        Task ShowProgressAsync();
        Task HideProgressAsync();
        Task CloseWebAppAsync();
        Task ListenMainButtonAsync();
        Task ListenBackButtonAsync();
        Task SetMainBtnColorAsync(string color);
        Task<InvoiceClosedStatus> InvoiceClosedHandlerAsync(string invoiceLink);
        Task SetHapticFeedbackImpactOccurredAsync(HapticFeedbackImpactOccurredType style);
        Task SetHapticFeedbackNotificationAsync(HapticFeedBackNotificationType type);
        Task<string> ShowPopupParamsAsync(PopupParams popup, HapticFeedBackNotificationType? type = null, HapticFeedbackImpactOccurredType? style = null);
    }
}
