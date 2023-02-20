using Microsoft.JSInterop;
using WebAppAssembly.Shared.Entities.Api.Common.OfTelegram;
using WebAppAssembly.Shared.Repositories.Common;

namespace TlgWebAppNet
{
    /// <summary>
    /// A class that facilitates working with the Web App for Telegram Bots API.
    /// Source: https://core.telegram.org/bots/webapps.
    /// </summary>
    public class TwaNet : ITwaNet
    {
        #region Fields

        private const string DefaultTelegramWebAppInit = "TelegramWebAppInit";
        private const string DefaultCloseWebApp = "CloseWebApp";
        private const string DefaultSetMainButtonText = "SetMainButtonText";
        private const string DefaultHideMainButton = "HideMainButton";
        private const string DefaultMainButtonHandler = "MainButtonHandler";
        private const string DefaultShowBackButton = "ShowBackButton";
        private const string DefaultHideBackButton = "HideBackButton";
        private const string DefaultBackButtonHandler = "BackButtonHandler";
        private const string DefaultInvoiceClosedHandler = "InvoiceClosedHandler";
        private const string DefaultSetHapticFeedbackNotification = "SetHapticFeedbackNotification";
        private const string DefaultSetHapticFeedbackSelectionChanged = "SetHapticFeedbackSelectionChanged";
        private const string DefaultSetOkPopupMessage = "SetOkPopupMessage";
        private const string DefaultShowProgress = "ShowProgress";
        private const string DefaultHideProgress = "HideProgress";
        private const string DefaultShowPopupParamsAsync = "ShowPopupParamsAsync";
        private const string DefaultSetHapticFeedbackImpactOccurred = "SetHapticFeedbackImpactOccurred";
        private const string DefaultSetMainButtonColor = "SetMainButtonColor";

        /// <summary>
        /// State of progressing.
        /// </summary>
        private bool _isProgressing;

        #endregion

        #region Properties

        public long ChatId { get; private set; }

        #endregion

        #region Constructors

        private TwaNet(long chatId)
            => ChatId = chatId;

        #endregion

        #region Methods

        public static async Task<TwaNet> CreateAsync(IJSRuntime jsRuntime)
            => new(await TwaNetInitAsync(jsRuntime));

        private static async Task<long> TwaNetInitAsync(IJSRuntime jsRuntime)
        {
            var chatId = await jsRuntime.InvokeAsync<long>(DefaultTelegramWebAppInit);
            if (chatId != 0) return chatId;
            throw new Exception($"Incorrect format of chat_id - '{chatId}'");
        }

        public async Task SetMainBtnColorAsync(IJSRuntime jsRuntime, string color)
            => await jsRuntime.InvokeVoidAsync(DefaultSetMainButtonColor, color);

        public async Task ListenMainButtonAsync(IJSRuntime jsRuntime)
            => await jsRuntime.InvokeVoidAsync(DefaultMainButtonHandler);

        public async Task ListenBackButtonAsync(IJSRuntime jsRuntime)
            => await jsRuntime.InvokeVoidAsync(DefaultBackButtonHandler);

        public async Task CloseWebAppAsync(IJSRuntime jsRuntime)
            => await jsRuntime.InvokeVoidAsync(DefaultCloseWebApp);

        public async Task SetMainButtonTextAsync(IJSRuntime jsRuntime, string txt)
            => await jsRuntime.InvokeVoidAsync(DefaultSetMainButtonText, txt, false);

        public static async Task SetMainButtonTextWithExpandAsync(IJSRuntime jsRuntime, string txt)
            => await jsRuntime.InvokeVoidAsync(DefaultSetMainButtonText, txt, true);

        public async Task HideMainButtonAsync(IJSRuntime jsRuntime)
            => await jsRuntime.InvokeVoidAsync(DefaultHideMainButton);

        public async Task ShowBackButtonAsync(IJSRuntime jsRuntime)
            => await jsRuntime.InvokeVoidAsync(DefaultShowBackButton);

        public async Task HideBackButtonAsync(IJSRuntime jsRuntime)
            => await jsRuntime.InvokeVoidAsync(DefaultHideBackButton);

        public async Task<InvoiceClosedStatus> InvoiceClosedHandlerAsync(IJSRuntime jsRuntime, string invoiceLink)
        {
            try
            {
                var res = await jsRuntime.InvokeAsync<string>(DefaultInvoiceClosedHandler, invoiceLink);

                if (!res.TryToConvertToInvoiceClosedStatus(out var invoiceClosedType))
                {
                    throw new Exception($"Failed to convert string value to {nameof(InvoiceClosedStatus)} type");
                }
                return (InvoiceClosedStatus)invoiceClosedType!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return InvoiceClosedStatus.Error;
            }
        }

        public async Task HapticFeedbackNotificationAsync(IJSRuntime jsRuntime, HapticFeedBackNotificationType type)
            => await jsRuntime.InvokeVoidAsync(DefaultSetHapticFeedbackNotification, type.EnumToString());

        public async Task HapticFeedbackImpactOccurredAsync(IJSRuntime jsRuntime, HapticFeedbackImpactOccurredType style)
            => await jsRuntime.InvokeVoidAsync(DefaultSetHapticFeedbackImpactOccurred, style.EnumToString());

        public async Task HapticFeedbackSelectionChangedAsync(IJSRuntime jsRuntime)
            => await jsRuntime.InvokeVoidAsync(DefaultSetHapticFeedbackSelectionChanged);

        public async Task ShowOkPopupMessageAsync(IJSRuntime jsRuntime, string title, string description, HapticFeedBackNotificationType notificationType)
        {
            await jsRuntime.InvokeVoidAsync(DefaultSetOkPopupMessage, title, description);
            await HapticFeedbackNotificationAsync(jsRuntime, notificationType);
        }

        public async Task ShowProgressAsync(IJSRuntime jsRuntime, bool leaveActive = false)
        {
            if (!_isProgressing)
            {
                await jsRuntime.InvokeVoidAsync(DefaultShowProgress, leaveActive);
                _isProgressing = true;
            }
        }

        public async Task HideProgressAsync(IJSRuntime jsRuntime)
        {
            if (_isProgressing)
            {
                await jsRuntime.InvokeVoidAsync(DefaultHideProgress);
                _isProgressing = false;
            }
        }

        public async Task<string> ShowPopupParamsAsync(IJSRuntime jsRuntime, PopupParams popup, HapticFeedBackNotificationType? type = null,
            HapticFeedbackImpactOccurredType? style = null)
        {
            if (type is not null) await HapticFeedbackNotificationAsync(jsRuntime, (HapticFeedBackNotificationType)type);

            var res = await jsRuntime.InvokeAsync<string>(DefaultShowPopupParamsAsync, popup.ToNativePopupParams());

            if (style is not null) await HapticFeedbackImpactOccurredAsync(jsRuntime, (HapticFeedbackImpactOccurredType)style);

            return res;
        }

        #endregion
    }
}