using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlgWebAppNet
{
    public interface ITwaNet
    {
        Task SetMainButtonTextAsync(string txt);
        Task SetHapticFeedbackSelectionChangedAsync();
        long GetChatId();
        Task ShowBackButtonAsync();
        Task HideMainButtonAsync();
    }
}
