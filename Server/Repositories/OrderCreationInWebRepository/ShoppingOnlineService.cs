using WebAppAssembly.Shared.Entities.EMenu;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using WebAppAssembly.Shared.Entities.CreateDelivery;
using WebAppAssembly.Shared.Entities.Exceptions;
using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.LogRepository;
using WebAppAssembly.Shared.Models.Order;
using ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID;
using ApiServerForTelegram.Entities.EExceptions;
using System.Reflection.Metadata.Ecma335;

namespace WebAppAssembly.Server.Repositories.OrderCreationOrderInWebRepository
{
    public class ShoppingOnlineService
    {
        public ShoppingOnlineService(IConfiguration configuration)
        {
            Console.WriteLine(typeof(ShoppingOnlineService).FullName!, nameof(ShoppingOnlineService), $"Initializing the {nameof(OrderInfo)} object and receiving the product items via db");
            Configuration = configuration;
            IsTestMode = Convert.ToBoolean(configuration["WebAppMode:TestMode"]);
            Url = new ApiServerUrls(configuration);

            try
            {
                var webAppInfoTask = GetDeliveryGeneralInfoAsync(Url.WebAppInfo);
                webAppInfoTask.Wait();

                DeliveryGeneralInfo = webAppInfoTask.Result;
                DeliveryTerminals = webAppInfoTask.Result.DeliveryTerminals ?? throw new Exception($"{typeof(ShoppingOnlineService).FullName}.{nameof(Exception)}: " +
                        $"{nameof(DeliveryTerminals)} can't be null");

                Console.WriteLine(typeof(ShoppingOnlineService).FullName!, nameof(ShoppingOnlineService), $"The {nameof(OrderInfo)} object is initialized and the product items is received");
            }
            catch (Exception ex)
            {
                Console.WriteLine(typeof(ShoppingOnlineService).FullName!, nameof(ShoppingOnlineService), LogService.FormatExceptionActionContent(ex));
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        private readonly IConfiguration Configuration;
        public OrderModelOfServer OrderInfo
        {
            get => OrderInfo is null ? throw new InfoException(typeof(ShoppingOnlineService).FullName!, nameof(Exception),
                typeof(OrderModelOfServer).FullName!, ExceptionType.NullOrEmpty) : OrderInfo;
            set { }
        }
        public bool IsReleaseMode { get; }
        private ApiServerUrls Url { get; }
        public DeliveryGeneralInfo DeliveryGeneralInfo { get; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public async Task<OrderModelOfServer> InitializeOrderModelAsync(long chatId)
        {
            var res = await GetOrderModelCashAsync(Url.OrderModel, chatId);
            return OrderInfo = res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        private static async Task<DeliveryGeneralInfo> GetDeliveryGeneralInfoAsync(string url)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.StatusCode.Equals(HttpStatusCode.OK))
                throw new HttpProcessException(response.StatusCode, responseBody);

            return JsonConvert.DeserializeObject<DeliveryGeneralInfo>(responseBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        private static async Task<OrderModelOfServer> GetOrderModelCashAsync(string url, long chatId)
        {
            using var httpClient = new HttpClient();
            var body = JsonConvert.SerializeObject(new { chatId });
            var data = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, data);

            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.StatusCode.Equals(HttpStatusCode.OK))
                throw new HttpProcessException(response.StatusCode, responseBody);

            return JsonConvert.DeserializeObject<OrderModelOfServer>(responseBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<double> GetCustomerWalletBalanceAsync(string url)
        {
            try
            {
                using var client = new HttpClient();
                string body = JsonConvert.SerializeObject(new { chatId = OrderInfo.ChatId });
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, data);
                response.EnsureSuccessStatusCode();

                // Get body from the response
                string responseBody = await response.Content.ReadAsStringAsync();
                if (!response.StatusCode.Equals(HttpStatusCode.OK))               
                    return 0;

                var customerWalletBalance = JsonConvert.DeserializeObject<CustomerDiscountBalance>(responseBody)
                    ?? throw new Exception($"{nameof(GetCustomerWalletBalanceAsync)}: This object can't be null");

                return customerWalletBalance.Balance;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        public async Task SendOrderInfoToServerAsync(OrderModelOfServer order)
        {
            OrderInfo = order;
            using var httpClient = new HttpClient();
            try
            {
                var body = JsonConvert.SerializeObject(order);
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Url.SaveOrderModel, data);

                string responseBody = await response.Content.ReadAsStringAsync();
                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                    throw new HttpProcessException(response.StatusCode, responseBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// Create invoice link in the server
        /// </summary>
        /// <returns></returns>
        public async Task<InvoiceLinkStatus> CreateInvoiceLinkAsync(OrderModelOfServer order)
        {
            OrderInfo = order;
            var orderModel = (OrderModelOfServer)OrderInfo.Clone();
            OrderModelForInvoice(ref orderModel);
            using var httpClient = new HttpClient();
            try
            {
                var body = JsonConvert.SerializeObject(new { chatId = ChatId, orderInfoViaWebApp = orderModel });
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Url.CheckOrderInfoAndCreateInvoiceLink, data);

                string responseBody = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new HttpProcessException(response.StatusCode, responseBody);

                return JsonConvert.DeserializeObject<InvoiceLinkStatus>(responseBody) ?? throw new Exception("Received an invoice link is invalid");
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
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        public async Task<LoyaltyCheckinInfo> CalculateCheckinAsync(OrderModelOfServer order)
        {
            try
            {
                OrderInfo = order;
                string responseBody = string.Empty;
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(DeliveryGeneralInfo.TimeOutForLoyaltyProgramProcessing);
                    var body = JsonConvert.SerializeObject(OrderInfo);
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
        /// Receive the wallet balance by chat ID
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="HttpProcessException"></exception>
        public async Task<WalletBalance> WalletBalanceAsync(ChatInfo chatInfo)
        {
            ChatId ??= chatInfo.ChatId;

            string responseBody = string.Empty;
            using (var client = new HttpClient())
            {
                var body = JsonConvert.SerializeObject(new { chatId = ChatId });
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Url.WalletBalance, data);

                responseBody = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new HttpProcessException(response.StatusCode, responseBody);
            }

            return JsonConvert.DeserializeObject<WalletBalance>(responseBody);
        }
    }
}