using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlgWebAppNet.Entities
{
    public enum TwaMethodNames
    {
        TelegramWebAppInit,
        CloseWebApp,
        SetMainButtonText,
        HideMainButton,
        MainButtonHandler,
        ShowBackButton,
        HideBackButton,
        BackButtonHandler,
        InvoiceClosedHandler,
        SetHapticFeedbackNotification,
        SetHapticFeedbackSelectionChanged,
        SetOkPopupMessage,
        ShowProgress,
        HideProgress,
        ShowPopupParamsAsync,
        SetHapticFeedbackImpactOccurred,
        SetMainButtonColor
    }
}
