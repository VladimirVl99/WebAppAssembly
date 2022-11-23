using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TlgWebAppNet.Entities;
using WebAppAssembly.Shared.Entities.Telegram;

namespace TlgWebAppNet
{
    public class TwaNet : ITwaNet
    {
        public TwaNet(IJSRuntime JsRuntime, string buttonColor)
        {
            this.JsRuntime = JsRuntime;
            var initTask = TlgWebAppInitAsync(buttonColor);
            initTask.Wait();
            ChatId = initTask.Result;
        }

        public TwaNet(IJSRuntime JsRuntime)
        {
            this.JsRuntime = JsRuntime;
            var initTask = TlgWebAppInitAsync();
            initTask.Wait();
            ChatId = initTask.Result;
        }

        private readonly IJSRuntime JsRuntime;
        public long ChatId { get; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<long> TlgWebAppInitAsync(string buttonColor)
        {
            await JsRuntime.InvokeVoidAsync(TwaMethodNames.SetMainButtonColor.ToString(), buttonColor);
            var chatId = await JsRuntime.InvokeAsync<long>(TwaMethodNames.TelegramWebAppInit.ToString());
            if (chatId != 0) return chatId;
            throw new Exception($"Incorrect format of chat_id - '{chatId}'");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<long> TlgWebAppInitAsync()
        {
            var chatId = await JsRuntime.InvokeAsync<long>(TwaMethodNames.TelegramWebAppInit.ToString());
            if (chatId != 0) return chatId;
            throw new Exception($"Incorrect format of chat_id - '{chatId}'");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ListenMainButtonAsync() => await JsRuntime.InvokeVoidAsync(TwaMethodNames.MainButtonHandler.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ListenBackButtonAsync() => await JsRuntime.InvokeVoidAsync(TwaMethodNames.BackButtonHandler.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task CloseWebAppAsync() => await JsRuntime.InvokeVoidAsync(TwaMethodNames.CloseWebApp.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task SetMainButtonTextAsync(string txt)
            => await JsRuntime.InvokeVoidAsync(TwaMethodNames.SetMainButtonText.ToString(), txt, false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public async Task SetMainButtonTextWithExpandAsync(string txt)
            => await JsRuntime.InvokeVoidAsync(TwaMethodNames.SetMainButtonText.ToString(), txt, true);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task HideMainButtonAsync() => await JsRuntime.InvokeVoidAsync(TwaMethodNames.HideMainButton.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ShowBackButtonAsync() => await JsRuntime.InvokeVoidAsync(TwaMethodNames.ShowBackButton.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task HideBackButtonAsync() => await JsRuntime.InvokeVoidAsync(TwaMethodNames.HideBackButton.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invoiceLink"></param>
        /// <returns></returns>
        public async Task<InvoiceClosedStatus> InvoiceClosedHandlerAsync(string invoiceLink)
        {
            try
            {
                var res = await JsRuntime.InvokeAsync<string>(TwaMethodNames.InvoiceClosedHandler.ToString(), invoiceLink);
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task SetHapticFeedbackNotificationAsync(HapticFeedBackNotificationType type)
            => await JsRuntime.InvokeVoidAsync(TwaMethodNames.SetHapticFeedbackNotification.ToString(), type.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task SetHapticFeedbackImpactOccurredAsync(HapticFeedbackImpactOccurredType style)
            => await JsRuntime.InvokeVoidAsync(TwaMethodNames.SetHapticFeedbackImpactOccurred.ToString(), style.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task SetHapticFeedbackSelectionChangedAsync()
            => await JsRuntime.InvokeVoidAsync(TwaMethodNames.SetHapticFeedbackSelectionChanged.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="notificationType"></param>
        /// <returns></returns>
        public async Task ShowOkPopupMessageAsync(string title, string description, HapticFeedBackNotificationType notificationType)
        {
            await JsRuntime.InvokeVoidAsync(TwaMethodNames.SetOkPopupMessage.ToString(), title, description);
            await SetHapticFeedbackNotificationAsync(notificationType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ShowProgressAsync() => await JsRuntime.InvokeVoidAsync(TwaMethodNames.ShowProgress.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task HideProgressAsync() => await JsRuntime.InvokeVoidAsync(TwaMethodNames.HideProgress.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="popup"></param>
        /// <returns></returns>
        public async Task<string> ShowPopupParamsAsync(PopupParams popup, HapticFeedBackNotificationType? type = null, HapticFeedbackImpactOccurredType? style = null)
        {
            if (type is not null) await SetHapticFeedbackNotificationAsync((HapticFeedBackNotificationType)type);

            var popupButtons = new List<PopupButtonAsString>();
            foreach (var popupButton in popup.Buttons)
                popupButtons.Add(new PopupButtonAsString(popupButton.Id, popupButton.Text,
                    popupButton.Type == PopupButtonType._default ? "default" : popupButton.Type.ToString()));

            var rightPopup = new PopupParamsAsString(popup.Title, popup.Message, popupButtons);
            var res = await JsRuntime.InvokeAsync<string>(TwaMethodNames.ShowPopupParamsAsync.ToString(), rightPopup);
            if (style is not null) await SetHapticFeedbackImpactOccurredAsync((HapticFeedbackImpactOccurredType)style);
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long GetChatId() => ChatId;
    }
}
