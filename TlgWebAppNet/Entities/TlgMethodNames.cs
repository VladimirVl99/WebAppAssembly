using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlgWebAppNet.Entities
{
    public class TlgMethodNames
    {
        public TlgMethodNames(string telegramWebAppInit, string closeWebApp, string setViewOrderButton, string setMainButton, string hideViewOrderButton,
            string setPayOrderButton, string setPriceOfSelectedProductWithModifiersButton, string mainButtonHandler, string mainButtonHandlerPromise,
            string backButtonShow, string backButtonHide, string backButtonHandler, string backButtonHandlerPromise, string invoiceHandler,
            string invoiceHandlerPromise, string setHapticFeedbackNotification, string setHapticFeedbackSelectionChanged, string setOkPopupMessage,
            string showProgress, string hideProgress, string scrollToTop)
        {
            TelegramWebAppInit = telegramWebAppInit;
            CloseWebApp = closeWebApp;
            SetViewOrderButton = setViewOrderButton;
            SetMainButton = setMainButton;
            HideViewOrderButton = hideViewOrderButton;
            SetPayOrderButton = setPayOrderButton;
            SetPriceOfSelectedProductWithModifiersButton = setPriceOfSelectedProductWithModifiersButton;
            MainButtonHandler = mainButtonHandler;
            MainButtonHandlerPromise = mainButtonHandlerPromise;
            BackButtonShow = backButtonShow;
            BackButtonHide = backButtonHide;
            BackButtonHandler = backButtonHandler;
            BackButtonHandlerPromise = backButtonHandlerPromise;
            InvoiceHandler = invoiceHandler;
            InvoiceHandlerPromise = invoiceHandlerPromise;
            SetHapticFeedbackNotification = setHapticFeedbackNotification;
            SetHapticFeedbackSelectionChanged = setHapticFeedbackSelectionChanged;
            SetOkPopupMessage = setOkPopupMessage;
            ShowProgress = showProgress;
            HideProgress = hideProgress;
            ScrollToTop = scrollToTop;
        }

        public string TelegramWebAppInit { get; }
        public string CloseWebApp { get; }
        public string SetViewOrderButton { get; }
        public string SetMainButton { get; }
        public string HideViewOrderButton { get; }
        public string SetPayOrderButton { get; }
        public string SetPriceOfSelectedProductWithModifiersButton { get; }
        public string MainButtonHandler { get; }
        public string MainButtonHandlerPromise { get; }
        public string BackButtonShow { get; }
        public string BackButtonHide { get; }
        public string BackButtonHandler { get; }
        public string BackButtonHandlerPromise { get; }
        public string InvoiceHandler { get; }
        public string InvoiceHandlerPromise { get; }
        public string SetHapticFeedbackNotification { get; }
        public string SetHapticFeedbackSelectionChanged { get; }
        public string SetOkPopupMessage { get; }
        public string ShowProgress { get; }
        public string HideProgress { get; }
        public string ScrollToTop { get; }
    }
}
