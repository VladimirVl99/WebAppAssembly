using ApiServerForTelegram.Entities.EExceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebAppAssembly.Shared.Entities;
using WebAppAssembly.Shared.Entities.CreateDelivery;
using WebAppAssembly.Shared.Entities.Exceptions;
using WebAppAssembly.Shared.Entities.Telegram;
using static System.Net.WebRequestMethods;

namespace WebAppAssembly.Shared.Models.Order.Service
{
    public class OrderService : IOrderService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="urlPathOfMainInfo"></param>
        /// <exception cref="InfoException"></exception>
        public OrderService(HttpClient client, long chatId, string urlPathOfMainInfo)
        {
            var mainInfoTask = RetrieveMainInfoForWebAppOrderAsync(client, urlPathOfMainInfo);
            mainInfoTask.Wait();
            var mainInfo = mainInfoTask.Result;

            OrderInfo = OrderInfoInit(mainInfo.OrderInfo);
            if (OrderInfo.ChatId == 0) SetTestChatIdToOrder(chatId);
            DeliveryGeneralInfo = mainInfo.DeliveryGeneralInfo ?? throw new InfoException(typeof(OrderService).FullName!,
                nameof(Exception), typeof(WebAppInfo).FullName!, ExceptionType.Null);
            IsDiscountBalanceConfirmed = false;
            IsReleaseMode = mainInfo.IsReleaseMode;
            var tlgMainBtnClr = !string.IsNullOrEmpty(mainInfo.TlgMainBtnColor)
                ? mainInfo.TlgMainBtnColor : mainInfo.TlgMainBtnColor ?? throw new InfoException(typeof(OrderService).FullName!,
                nameof(Exception), $"{typeof(MainInfoForWebAppOrder).FullName!}.{nameof(MainInfoForWebAppOrder.TlgMainBtnColor)}", ExceptionType.NullOrEmpty);
            TlgMainBtnColor = IsRgbColorFormat(tlgMainBtnClr) ? mainInfo.TlgMainBtnColor : throw new InfoException(typeof(OrderService).FullName!,
                nameof(Exception), $"Incorrect formant of rgb (#rrggbb) color for the main button of the Telegram. Current value is '{tlgMainBtnClr}'");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainInfo"></param>
        /// <exception cref="InfoException"></exception>
        public OrderService(MainInfoForWebAppOrder mainInfo, long chatId)
        {
            OrderInfo = OrderInfoInit(mainInfo.OrderInfo);
            if (OrderInfo.ChatId == 0) SetTestChatIdToOrder(chatId);
            DeliveryGeneralInfo = mainInfo.DeliveryGeneralInfo ?? throw new InfoException(typeof(OrderService).FullName!,
                    nameof(Exception), typeof(WebAppInfo).FullName!, ExceptionType.Null);
            IsDiscountBalanceConfirmed = false;
            IsReleaseMode = mainInfo.IsReleaseMode;
            var tlgMainBtnClr = !string.IsNullOrEmpty(mainInfo.TlgMainBtnColor)
                ? mainInfo.TlgMainBtnColor : mainInfo.TlgMainBtnColor ?? throw new InfoException(typeof(OrderService).FullName!,
                nameof(Exception), $"{typeof(MainInfoForWebAppOrder).FullName!}.{nameof(MainInfoForWebAppOrder.TlgMainBtnColor)}", ExceptionType.NullOrEmpty);
            TlgMainBtnColor = IsRgbColorFormat(tlgMainBtnClr) ? mainInfo.TlgMainBtnColor : throw new InfoException(typeof(OrderService).FullName!,
                nameof(Exception), $"Incorrect formant of rgb (#rrggbb) color for the main button of the Telegram. Current value is '{tlgMainBtnClr}'");
        }

        public OrderModel OrderInfo { get; set; }
        public WebAppInfo DeliveryGeneralInfo { get; set; }
        public bool IsDiscountBalanceConfirmed { get; set; }
        public CurrentProduct? CurrentProduct { get; set; }
        public Guid? CurrentGroupId { get; set; }
        public bool IsReleaseMode { get; set; }
        public string TlgMainBtnColor { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="urlPathOfMainInfo"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        private static async Task<MainInfoForWebAppOrder> RetrieveMainInfoForWebAppOrderAsync(HttpClient client, string urlPathOfMainInfo)
        {
            var response = await client.GetAsync(urlPathOfMainInfo);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.StatusCode.Equals(HttpStatusCode.OK))
                throw new HttpProcessException(response.StatusCode, responseBody);

            return JsonConvert.DeserializeObject<MainInfoForWebAppOrder>(responseBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private static bool IsRgbColorFormat(string color)
        {
            const int rgbFormatLength = 7;
            string clrCopy = new(color);

            if (clrCopy.Length != rgbFormatLength && !clrCopy.StartsWith('#'))
                return false;

            clrCopy = clrCopy.Remove(0);
            foreach (var chr in clrCopy)
                if (!char.IsDigit(chr)) return false;

            return true;
        }

        /// <summary>
        /// !!! Redone it for better !!!
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        private OrderModel OrderInfoInit(OrderModel? orderInfo = null)
        {
            if (orderInfo is not null)
            {
                orderInfo.Items ??= new List<Item>();

                if (!orderInfo.Items.Any())
                    orderInfo.WithNewParameters();
                else
                {
                    var items = orderInfo.Items;
                    var products = DeliveryGeneralInfo.TransportItemDtos;
                    if (products is not null)
                        foreach (var item in items) products.First(y => y.ItemId == item.ProductId).TotalAmount += (int)item.Amount;
                }
                orderInfo.FreeItems ??= new List<Item>();
                orderInfo.FreePriceItems ??= new List<Guid>();
                return orderInfo;
            }
            else return new OrderModel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatId"></param>
        private void SetTestChatIdToOrder(long chatId) => OrderInfo.ChatId = chatId;
    }
}