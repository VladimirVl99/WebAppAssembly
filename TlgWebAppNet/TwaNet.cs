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
        public TwaNet()
        {
            IsProgressing = false;
        }

        public long ChatId { get; private set; }
        private bool IsProgressing { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<long> TwaNetInitAsync(IJSRuntime jsRuntime) => ChatId = await TlgWebAppInitAsync(jsRuntime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="btnColor"></param>
        /// <returns></returns>
        public async Task<long> TwaNetInitAsync(IJSRuntime jsRuntime, string btnColor)
            => ChatId = await TlgWebAppInitAsync(jsRuntime, btnColor);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<long> TlgWebAppInitAsync(IJSRuntime jsRuntime, string buttonColor)
        {
            await jsRuntime.InvokeVoidAsync(TwaMethodNames.SetMainButtonColor.ToString(), buttonColor);
            var chatId = await jsRuntime.InvokeAsync<long>(TwaMethodNames.TelegramWebAppInit.ToString());
            if (chatId != 0) return chatId;
            throw new Exception($"Incorrect format of chat_id - '{chatId}'");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<long> TlgWebAppInitAsync(IJSRuntime jsRuntime)
        {
            var chatId = await jsRuntime.InvokeAsync<long>(TwaMethodNames.TelegramWebAppInit.ToString());
            if (chatId != 0) return chatId;
            throw new Exception($"Incorrect format of chat_id - '{chatId}'");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public async Task SetMainBtnColorAsync(IJSRuntime jsRuntime, string color)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.SetMainButtonColor.ToString(), color);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ListenMainButtonAsync(IJSRuntime jsRuntime) => await jsRuntime.InvokeVoidAsync(TwaMethodNames.MainButtonHandler.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ListenBackButtonAsync(IJSRuntime jsRuntime) => await jsRuntime.InvokeVoidAsync(TwaMethodNames.BackButtonHandler.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task CloseWebAppAsync(IJSRuntime jsRuntime) => await jsRuntime.InvokeVoidAsync(TwaMethodNames.CloseWebApp.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task SetMainButtonTextAsync(IJSRuntime jsRuntime, string txt)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.SetMainButtonText.ToString(), txt, false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public async Task SetMainButtonTextWithExpandAsync(IJSRuntime jsRuntime, string txt)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.SetMainButtonText.ToString(), txt, true);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task HideMainButtonAsync(IJSRuntime jsRuntime) => await jsRuntime.InvokeVoidAsync(TwaMethodNames.HideMainButton.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ShowBackButtonAsync(IJSRuntime jsRuntime) => await jsRuntime.InvokeVoidAsync(TwaMethodNames.ShowBackButton.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task HideBackButtonAsync(IJSRuntime jsRuntime) => await jsRuntime.InvokeVoidAsync(TwaMethodNames.HideBackButton.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invoiceLink"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task SetHapticFeedbackNotificationAsync(IJSRuntime jsRuntime, HapticFeedBackNotificationType type)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.SetHapticFeedbackNotification.ToString(), type.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task SetHapticFeedbackImpactOccurredAsync(IJSRuntime jsRuntime, HapticFeedbackImpactOccurredType style)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.SetHapticFeedbackImpactOccurred.ToString(), style.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task SetHapticFeedbackSelectionChangedAsync(IJSRuntime jsRuntime)
            => await jsRuntime.InvokeVoidAsync(TwaMethodNames.SetHapticFeedbackSelectionChanged.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="notificationType"></param>
        /// <returns></returns>
        public async Task ShowOkPopupMessageAsync(IJSRuntime jsRuntime, string title, string description, HapticFeedBackNotificationType notificationType)
        {
            await jsRuntime.InvokeVoidAsync(TwaMethodNames.SetOkPopupMessage.ToString(), title, description);
            await SetHapticFeedbackNotificationAsync(jsRuntime, notificationType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ShowProgressAsync(IJSRuntime jsRuntime)
        {
            if (!IsProgressing)
            {
                await jsRuntime.InvokeVoidAsync(TwaMethodNames.ShowProgress.ToString());
                IsProgressing = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task HideProgressAsync(IJSRuntime jsRuntime)
        {
            if (IsProgressing)
            {
                await jsRuntime.InvokeVoidAsync(TwaMethodNames.HideProgress.ToString());
                IsProgressing = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="popup"></param>
        /// <returns></returns>
        public async Task<string> ShowPopupParamsAsync(IJSRuntime jsRuntime, PopupParams popup, HapticFeedBackNotificationType? type = null,
            HapticFeedbackImpactOccurredType? style = null)
        {
            if (type is not null) await SetHapticFeedbackNotificationAsync(jsRuntime, (HapticFeedBackNotificationType)type);

            var popupButtons = new List<PopupButtonAsString>();
            foreach (var popupButton in popup.Buttons)
                popupButtons.Add(new PopupButtonAsString(popupButton.Id, popupButton.Text,
                    popupButton.Type == PopupButtonType._default ? "default" : popupButton.Type.ToString()));

            var rightPopup = new PopupParamsAsString(popup.Title, popup.Message, popupButtons);
            var res = await jsRuntime.InvokeAsync<string>(TwaMethodNames.ShowPopupParamsAsync.ToString(), rightPopup);
            if (style is not null) await SetHapticFeedbackImpactOccurredAsync(jsRuntime, (HapticFeedbackImpactOccurredType)style);
            return res;
        }
    }
}