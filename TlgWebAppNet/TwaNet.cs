using Microsoft.JSInterop;
using TlgWebAppNet.Entities;
using WebAppAssembly.Shared.Entities.Telegram;

namespace TlgWebAppNet
{
    /// <summary>
    /// A class that facilitates working with the Web App for Telegram Bots API.
    /// Source: https://core.telegram.org/bots/webapps.
    /// </summary>
    public class TwaNet : ITwaNet
    {
        #region Fields

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
            var chatId = await jsRuntime.InvokeAsync<long>(TwaMethodNames.TelegramWebAppInit.ToString());
            if (chatId != 0) return chatId;
            throw new Exception($"Incorrect format of chat_id - '{chatId}'");
        }

        public async Task SetMainBtnColorAsync(IJSRuntime jsRuntime, string color)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.SetMainButtonColor.ToString(), color);

        public async Task ListenMainButtonAsync(IJSRuntime jsRuntime)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.MainButtonHandler.ToString());

        public async Task ListenBackButtonAsync(IJSRuntime jsRuntime)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.BackButtonHandler.ToString());

        public async Task CloseWebAppAsync(IJSRuntime jsRuntime)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.CloseWebApp.ToString());

        public async Task SetMainButtonTextAsync(IJSRuntime jsRuntime, string txt)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.SetMainButtonText.ToString(), txt, false);

        public static async Task SetMainButtonTextWithExpandAsync(IJSRuntime jsRuntime, string txt)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.SetMainButtonText.ToString(), txt, true);

        public async Task HideMainButtonAsync(IJSRuntime jsRuntime)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.HideMainButton.ToString());

        public async Task ShowBackButtonAsync(IJSRuntime jsRuntime)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.ShowBackButton.ToString());

        public async Task HideBackButtonAsync(IJSRuntime jsRuntime)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.HideBackButton.ToString());

        public async Task<InvoiceClosedStatus> InvoiceClosedHandlerAsync(IJSRuntime jsRuntime, string invoiceLink)
        {
            try
            {
                var res = await jsRuntime.InvokeAsync<string>(TwaMethodNames.InvoiceClosedHandler.ToString(), invoiceLink);
                if (!Enum.TryParse(res, out InvoiceClosedStatus invoiceClosedType))
                    throw new Exception($"Failed to convert string value to {nameof(InvoiceClosedStatus)} type");
                return invoiceClosedType;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return InvoiceClosedStatus.error;
            }
        }

        public async Task HapticFeedbackNotificationAsync(IJSRuntime jsRuntime, HapticFeedBackNotificationType type)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.SetHapticFeedbackNotification.ToString(), type.ToString());

        public async Task HapticFeedbackImpactOccurredAsync(IJSRuntime jsRuntime, HapticFeedbackImpactOccurredType style)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.SetHapticFeedbackImpactOccurred.ToString(), style.ToString());

        public async Task HapticFeedbackSelectionChangedAsync(IJSRuntime jsRuntime)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.SetHapticFeedbackSelectionChanged.ToString());

        public async Task ShowOkPopupMessageAsync(IJSRuntime jsRuntime, string title, string description, HapticFeedBackNotificationType notificationType)
        {
            await jsRuntime.InvokeVoidAsync(TwaMethodNames.SetOkPopupMessage.ToString(), title, description);
            await HapticFeedbackNotificationAsync(jsRuntime, notificationType);
        }

        public async Task ShowProgressAsync(IJSRuntime jsRuntime, bool leaveActive = false)
        {
            if (!_isProgressing)
            {
                await jsRuntime.InvokeVoidAsync(TwaMethodNames.ShowProgress.ToString(), leaveActive);
                _isProgressing = true;
            }
        }

        public async Task HideProgressAsync(IJSRuntime jsRuntime)
        {
            if (_isProgressing)
            {
                await jsRuntime.InvokeVoidAsync(TwaMethodNames.HideProgress.ToString());
                _isProgressing = false;
            }
        }

        public async Task<string> ShowPopupParamsAsync(IJSRuntime jsRuntime, PopupParams popup, HapticFeedBackNotificationType? type = null,
            HapticFeedbackImpactOccurredType? style = null)
        {
            if (type is not null) await HapticFeedbackNotificationAsync(jsRuntime, (HapticFeedBackNotificationType)type);

            var popupButtons = new List<PopupButtonAsString>();
            foreach (var popupButton in popup.Buttons)
                popupButtons.Add(new PopupButtonAsString(popupButton.Id, popupButton.Text,
                    popupButton.Type == PopupButtonType._default ? "default" : popupButton.Type.ToString()));

            var rightPopup = new PopupParamsAsString(popup.Title, popup.Message, popupButtons);
            var res = await jsRuntime.InvokeAsync<string>(TwaMethodNames.ShowPopupParamsAsync.ToString(), rightPopup);
            if (style is not null) await HapticFeedbackImpactOccurredAsync(jsRuntime, (HapticFeedbackImpactOccurredType)style);
            return res;
        }

        #endregion
    }
}