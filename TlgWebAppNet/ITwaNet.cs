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
        Task SetMainButtonTextAsync(string txt);
        Task SetHapticFeedbackSelectionChangedAsync();
        long GetChatId();
        Task ShowBackButtonAsync();
        Task HideMainButtonAsync();
        Task ShowOkPopupMessageAsync(string title, string description, HapticFeedBackNotificationType notificationType);
        Task ShowProgressAsync();
        Task HideProgressAsync();
    }
}
