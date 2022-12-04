using Newtonsoft.Json;
using System.Net;
using System.Text;
using WebAppAssembly.Shared.Entities.Exceptions;
using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.LogRepository;
using WebAppAssembly.Shared.Models.Order;
using WebAppAssembly.Server.Repositories.OrderCreationInWebRepository;

namespace WebAppAssembly.Server.Repositories.OrderCreationOrderInWebRepository
{
    public class ShoppingOnlineService : IShoppingOnlineService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public ShoppingOnlineService(IConfiguration configuration)
        {
            Configuration = configuration;
            IsReleaseMode = !Convert.ToBoolean(configuration["WebAppMode:TestMode"]);
            Url = new ApiServerUrls(configuration);

            try
            {
                var deliveryGeneralInfoTask = GetDeliveryGeneralInfoAsync();
                deliveryGeneralInfoTask.Wait();
                DeliveryGeneralInfo = deliveryGeneralInfoTask.Result;
                if (string.IsNullOrEmpty(DeliveryGeneralInfo.TlgMainBtnColor))
                    DeliveryGeneralInfo.TlgMainBtnColor = configuration["Settings:TlgMainButtonColor"];
                DeliveryGeneralInfo.TimeOutForLoyaltyProgramProcessing ??= Convert.ToDouble(configuration["Settings:TimeOutForLoyaltyProgramProcessing"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(typeof(ShoppingOnlineService).FullName!, nameof(ShoppingOnlineService), LogService.FormatExceptionActionContent(ex));
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        private readonly IConfiguration Configuration;
        public bool IsReleaseMode { get; }
        private ApiServerUrls Url { get; }
        public DeliveryGeneralInfo DeliveryGeneralInfo { get; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        private async Task<DeliveryGeneralInfo> GetDeliveryGeneralInfoAsync()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(Url.WebAppInfo);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.StatusCode.Equals(HttpStatusCode.OK))
                throw new HttpProcessException(response.StatusCode, responseBody);

            return JsonConvert.DeserializeObject<DeliveryGeneralInfo>(responseBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        public async Task<OrderModelOfServer> GetOrderModelCashAsync(ChatInfo chatInfo)
        {
            using var httpClient = new HttpClient();
            var body = JsonConvert.SerializeObject(chatInfo);
            var data = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(Url.OrderModel, data);

            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.StatusCode.Equals(HttpStatusCode.OK))
                throw new HttpProcessException(response.StatusCode, responseBody);

            return JsonConvert.DeserializeObject<OrderModelOfServer>(responseBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        public async Task SendOrderInfoToServerAsync(OrderModelOfServer order)
        {
            try
            {
                using var client = new HttpClient();
                var body = JsonConvert.SerializeObject(order);
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Url.SaveOrderModel, data);

                string responseBody = await response.Content.ReadAsStringAsync();
                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                    throw new HttpProcessException(response.StatusCode, responseBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        public async Task<InvoiceLinkStatus> CreateInvoiceLinkAsync(OrderModelOfServer order)
        {;
            try
            {
                using var client = new HttpClient();
                var body = JsonConvert.SerializeObject(order);
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Url.CheckOrderInfoAndCreateInvoiceLink, data);

                string responseBody = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new HttpProcessException(response.StatusCode, responseBody);

                return JsonConvert.DeserializeObject<InvoiceLinkStatus>(responseBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new(false, null, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        public async Task<LoyaltyCheckinInfo> CalculateCheckinAsync(OrderModelOfServer order)
        {
            try
            {
                string responseBody = string.Empty;
                using (var client = new HttpClient())
                {
                    if (DeliveryGeneralInfo.TimeOutForLoyaltyProgramProcessing is not null)
                        client.Timeout = TimeSpan.FromSeconds((double)DeliveryGeneralInfo.TimeOutForLoyaltyProgramProcessing);

                    var body = JsonConvert.SerializeObject(order);
                    var data = new StringContent(body, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(Url.Checkin, data);

                    responseBody = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new HttpProcessException(response.StatusCode, responseBody);
                }

                var checkin = JsonConvert.DeserializeObject<Checkin>(responseBody);
                return new LoyaltyCheckinInfo(true, checkin);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return new LoyaltyCheckinInfo(
                    ok: false,
                    httpResponseInfo: new HttpResponseInfo(ex.StatusCode ?? HttpStatusCode.InternalServerError, ex.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new LoyaltyCheckinInfo(
                ok: false,
                httpResponseInfo: new HttpResponseInfo(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatInfo"></param>
        /// <returns></returns>
        public async Task<WalletBalance> GetCustomerWalletBalanceAsync(ChatInfo chatInfo)
        {
            try
            {
                using var client = new HttpClient();
                string body = JsonConvert.SerializeObject(chatInfo);
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Url.WalletBalance, data);
                response.EnsureSuccessStatusCode();

                // Get body from the response
                string responseBody = await response.Content.ReadAsStringAsync();
                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                    return new WalletBalance() { Balance = 0 };

                return JsonConvert.DeserializeObject<WalletBalance>(responseBody);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return new WalletBalance() { Balance = 0 };
            }
        }
    }
}