using Newtonsoft.Json;
using System.Net;
using System.Text;
using WebAppAssembly.Shared.Entities.Exceptions;
using WebAppAssembly.Shared.LogRepository;
using WebAppAssembly.Shared.Entities.Api.Common.PersonalData;
using WebAppAssembly.Shared.Entities.Api.Common.OnlineStore;
using WebAppAssembly.Shared.Entities.Api.Common.OfTelegram;
using InvoiceLinkRequest = WebAppAssembly.Shared.Entities.Api.Requests.OfTelegram.InvoiceLinkStatus;
using WebAppAssembly.Shared.Entities.Api.Common.Loylties;
using LoyaltyCheckInfoRequest = WebAppAssembly.Shared.Entities.Api.Requests.Loyalties.LoyaltyCheckinInfo;
using HttpResInfoRequest = WebAppAssembly.Shared.Entities.Api.Requests.HttpInfos.HttpResponseShortInfo;
using WebAppAssembly.Server.Entities.Urls;

namespace WebAppAssembly.Server.Repositories.ForOnlineStore
{
    /// <summary>
    /// For working with an online store
    /// </summary>
    public class OnlineStore : IOnlineStore
    {
        /// <summary>
        /// Initializes the necessary data for the operation of web application
        /// </summary>
        /// <param name="configuration"></param>
        public OnlineStore(IConfiguration configuration)
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
                OnlineStoreItem = deliveryGeneralInfoTask.Result;

                // Gets and sets a default color for the Tg's main button
                if (string.IsNullOrWhiteSpace(OnlineStoreItem.TgMainBtnColor))
                    OnlineStoreItem.TgMainBtnColor = configuration["Settings:TlgMainButtonColor"];
                // Gets and sets a default time out for processing the any loyalty program operations
                OnlineStoreItem.TimeOutForLoyaltyProgramProcessing ??= Convert.ToDouble(configuration["Settings:TimeOutForLoyaltyProgramProcessing"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(typeof(OnlineStore).FullName!, nameof(OnlineStore), LogService.FormatExceptionActionContent(ex));
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
        public OnlineStoreItem OnlineStoreItem { get; }


        /// <summary>
        /// Gets the necessary information for the operation of an online store from the API server
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        private async Task<OnlineStoreItem> GetGenralInfoForOnlineStoreAsync()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(Url.CommonDataForOnlineStore);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.StatusCode.Equals(HttpStatusCode.OK))
                throw new HttpProcessException(response.StatusCode, responseBody);

            return JsonConvert.DeserializeObject<OnlineStoreItem>(responseBody);
        }

        /// <summary>
        /// Gets a customer's personal data of the order from the API server.
        /// For example: selected products, a selected delivery method, an address and etc.
        /// </summary>
        /// <param name="chatInfo"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        public async Task<PersonalInfo> GetPersonalDataOfOrderAsync(TgChat chatInfo)
        {
            using var httpClient = new HttpClient();
            var body = JsonConvert.SerializeObject(chatInfo);
            var data = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(Url.PersonalInfo, data);

            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.StatusCode.Equals(HttpStatusCode.OK))
                throw new HttpProcessException(response.StatusCode, responseBody);

            return JsonConvert.DeserializeObject<PersonalInfo>(responseBody);
        }

        /// <summary>
        /// Saves the changed personal data in API server
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        public async Task SavePersonalDataOfOrderInServerAsync(PersonalInfo order)
        {
            try
            {
                using var client = new HttpClient();
                var body = JsonConvert.SerializeObject(order);
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Url.SavePersonalInfo, data);

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
        public async Task<InvoiceLinkStatus> RetrieveInvoiceLinkAsync(PersonalInfo order)
        {
            try
            {
                using var client = new HttpClient();
                var body = JsonConvert.SerializeObject(order);
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Url.CheckPersonalOrderAndCreateInvoiceLink, data);

                string responseBody = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new HttpProcessException(response.StatusCode, responseBody);

                return JsonConvert.DeserializeObject<InvoiceLinkStatus>(responseBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new InvoiceLinkRequest(ok: false, message: ex.Message);
            }
        }

        /// <summary>
        /// Caluculates the checkin for the order (loaylty program).
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        public async Task<LoyaltyCheckinInfo> CalculateCheckinAsync(PersonalInfo order)
        {
            try
            {
                string responseBody = string.Empty;
                using (var client = new HttpClient())
                {
                    if (OnlineStoreItem.TimeOutForLoyaltyProgramProcessing is not null)
                        client.Timeout = TimeSpan.FromSeconds((double)OnlineStoreItem.TimeOutForLoyaltyProgramProcessing);

                    var body = JsonConvert.SerializeObject(order);
                    var data = new StringContent(body, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(Url.Checkin, data);

                    responseBody = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new HttpProcessException(response.StatusCode, responseBody);
                }

                var checkin = JsonConvert.DeserializeObject<Checkin>(responseBody);
                return new LoyaltyCheckInfoRequest(true, checkin);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return new LoyaltyCheckInfoRequest(
                    ok: false,
                    httpResponseInfo: new HttpResInfoRequest(ex.StatusCode ?? HttpStatusCode.InternalServerError, ex.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new LoyaltyCheckInfoRequest(
                ok: false,
                httpResponseInfo: new HttpResInfoRequest(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Receives the wallet balance of a customer by the chat info
        /// </summary>
        /// <param name="chatInfo"></param>
        /// <returns></returns>
        public async Task<WalletBalance> GetCustomerWalletBalanceAsync(TgChat chatInfo)
        {
            try
            {
                using var client = new HttpClient();
                if (OnlineStoreItem.TimeOutForLoyaltyProgramProcessing is not null)
                    client.Timeout = TimeSpan.FromSeconds((double)OnlineStoreItem.TimeOutForLoyaltyProgramProcessing);

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