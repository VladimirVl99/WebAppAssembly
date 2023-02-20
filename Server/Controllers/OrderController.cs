using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAppAssembly.Server.Repositories.ForOnlineStore;
using WebAppAssembly.Shared.Entities.Api.Common.Loylties;
using WebAppAssembly.Shared.Entities.Api.Common.OfTelegram;
using WebAppAssembly.Shared.Entities.Api.Common.OnlineStore;
using WebAppAssembly.Shared.Entities.Api.Common.PersonalData;
using OnlineStoreInfoRequest = WebAppAssembly.Shared.Entities.Api.Requests.OnlineStore.OnlineStoreInfo;

namespace WebAppAssembly.Server.Controllers
{
    /// <summary>
    /// Controller for online store.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        /// <summary>
        /// Initializes the object for working with order infos
        /// </summary>
        /// <param name="configuration"></param>
        public OrderController(IConfiguration configuration)
        {
            ShoppingOrderService = new OnlineStore(configuration);
        }

        /// <summary>
        /// For working with order infos
        /// </summary>
        private readonly IOnlineStore ShoppingOrderService;


        /// <summary>
        /// Gets a necessary general data for the operation of the web application and gets a personal data of a customer,
        /// for example: selected products, a selected delivery method, an address and etc.
        /// </summary>
        /// <param name="chatInfo"></param>
        /// <returns></returns>
        [HttpPost("mainInfoForWebAppOrder")]
        public async Task<ActionResult<OnlineStoreInfo>> MainInfoOfOnlineStoreAsync(TgChat chatInfo)
        {
            try
            {
                // Gets a personal data of a customer via API server
                var personalOrderInfo = await ShoppingOrderService.GetPersonalDataOfOrderAsync(chatInfo);
                // Returns the gotten personal data of a customer, the general data for the operation of the web app and the web app operation mode
                return Ok(new OnlineStoreInfoRequest(ShoppingOrderService.OnlineStoreItem, ShoppingOrderService.IsReleaseMode)
                {
                    PersonalOrderInfo = personalOrderInfo
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Receives the wallet balance of a customer by the chat info
        /// </summary>
        /// <param name="chatInfo"></param>
        /// <returns></returns>
        [HttpPost("walletBalance")]
        public async Task<ActionResult<WalletBalance>> WalletBalanceOfCustomerAsync(TgChat chatInfo)
        {
            try
            {
                return Ok(await ShoppingOrderService.GetCustomerWalletBalanceAsync(chatInfo));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Saves the changed personal data in API server
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost("saveOrderInfoInServer")]
        public async Task<IActionResult> SavePersonalDataOfOrderInServerAsync(PersonalInfo order)
        {
            try
            {
                await ShoppingOrderService.SavePersonalDataOfOrderInServerAsync(order);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Caluculates the checkin for the order (loaylty program)
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost("calculateCheckin")]
        public async Task<ActionResult<LoyaltyCheckinInfo>> CalculateCheckinAsync(PersonalInfo order)
        {
            try
            {
                return Ok(await ShoppingOrderService.CalculateCheckinAsync(order));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets an invoice link to pay the order in the Telegram interface
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost("createInvoiceLink")]
        public async Task<ActionResult<InvoiceLinkStatus>> RetrieveInvoiceLinkAsync(PersonalInfo order)
        {
            try
            {
                return Ok(await ShoppingOrderService.RetrieveInvoiceLinkAsync(order));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
