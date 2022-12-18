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
    /// <summary>
    /// For working with an online store
    /// </summary>
    public class ShoppingOnlineService : IShoppingOnlineService
    {
        /// <summary>
        /// Initializes the necessary data for the operation of web application
        /// </summary>
        /// <param name="configuration"></param>
        public ShoppingOnlineService(IConfiguration configuration)
        {
            try
            {
                // An operation mode of the web application
                IsReleaseMode = !Convert.ToBoolean(configuration["WebAppMode:TestMode"]);
                // Urls for working with the API server
                Url = new ApiServerUrls(configuration);

                // Retrieves the necessary information for the operation of an online store
                var deliveryGeneralInfoTask = GetGenralInfoForOnlineStoreAsync();
                deliveryGeneralInfoTask.Wait();
                GeneralInfoOfOnlineStore = deliveryGeneralInfoTask.Result;

                // Gets and sets a default color for the Tg's main button
                if (string.IsNullOrEmpty(GeneralInfoOfOnlineStore.TlgMainBtnColor))
                    GeneralInfoOfOnlineStore.TlgMainBtnColor = configuration["Settings:TlgMainButtonColor"];
                // Gets and sets a default time out for processing the any loyalty program operations
                GeneralInfoOfOnlineStore.TimeOutForLoyaltyProgramProcessing ??= Convert.ToDouble(configuration["Settings:TimeOutForLoyaltyProgramProcessing"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(typeof(ShoppingOnlineService).FullName!, nameof(ShoppingOnlineService), LogService.FormatExceptionActionContent(ex));
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// The web application operation mode (test or release mode)
        /// </summary>
        public bool IsReleaseMode { get; }
        /// <summary>
        /// Urls for working with the API server
        /// </summary>
        private ApiServerUrls Url { get; }
        /// <summary>
        /// Stores the necessary information for the operation of an online store.
        /// It stores information about prouducts, product categories, dilivery methods, points of sale,
        /// loyalty program and etc.
        /// </summary>
        public GeneralInfoOfOnlineStore GeneralInfoOfOnlineStore { get; }


        /// <summary>
        /// Gets the necessary information for the operation of an online store from the API server
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        private async Task<GeneralInfoOfOnlineStore> GetGenralInfoForOnlineStoreAsync()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(Url.WebAppInfo);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.StatusCode.Equals(HttpStatusCode.OK))
                throw new HttpProcessException(response.StatusCode, responseBody);

            return JsonConvert.DeserializeObject<GeneralInfoOfOnlineStore>(responseBody);
        }

        /// <summary>
        /// Gets a customer's personal data of the order.
        /// For example: selected products, a selected delivery method, an address and etc.
        /// </summary>
        /// <param name="chatInfo"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        public async Task<PersonalInfoOfOrderByServerSide> GetPersonalDataOfOrderAsync(ChatInfo chatInfo)
        {
            using var httpClient = new HttpClient();
            var body = JsonConvert.SerializeObject(chatInfo);
            var data = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(Url.OrderModel, data);

            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.StatusCode.Equals(HttpStatusCode.OK))
                throw new HttpProcessException(response.StatusCode, responseBody);

            return JsonConvert.DeserializeObject<PersonalInfoOfOrderByServerSide>(responseBody);
        }

        /// <summary>
        /// Saves the changed personal data in API server
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        public async Task SavePersonalDataOfOrderInServerAsync(PersonalInfoOfOrderByServerSide order)
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
        /// Gets an invoice link to pay the order in the Telegram interface
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        public async Task<InvoiceLinkStatus> RetrieveInvoiceLinkAsync(PersonalInfoOfOrderByServerSide order)
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
        /// Caluculates the checkin for the order (loaylty program)
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        public async Task<LoyaltyCheckinInfo> CalculateCheckinAsync(PersonalInfoOfOrderByServerSide order)
        {
            try
            {
                string responseBody = string.Empty;
                using (var client = new HttpClient())
                {
                    if (GeneralInfoOfOnlineStore.TimeOutForLoyaltyProgramProcessing is not null)
                        client.Timeout = TimeSpan.FromSeconds((double)GeneralInfoOfOnlineStore.TimeOutForLoyaltyProgramProcessing);

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
        /// Receives the wallet balance of a customer by the chat info
        /// </summary>
        /// <param name="chatInfo"></param>
        /// <returns></returns>
        public async Task<WalletBalance> GetCustomerWalletBalanceAsync(ChatInfo chatInfo)
        {
            try
            {
                using var client = new HttpClient();
                if (GeneralInfoOfOnlineStore.TimeOutForLoyaltyProgramProcessing is not null)
                    client.Timeout = TimeSpan.FromSeconds((double)GeneralInfoOfOnlineStore.TimeOutForLoyaltyProgramProcessing);

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