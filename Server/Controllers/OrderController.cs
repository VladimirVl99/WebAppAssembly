using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAppAssembly.Server.Repositories.OrderCreationInWebRepository;
using WebAppAssembly.Server.Repositories.OrderCreationOrderInWebRepository;
using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.Models.Order;
using MainInfoForWebAppOrderOfServerSide = WebAppAssembly.Shared.Entities.OfServerSide.MainInfoForWebAppOrder;

namespace WebAppAssembly.Server.Controllers
{
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
            ShoppingOrderService = new ShoppingOnlineService(configuration);
        }

        /// <summary>
        /// For working with order infos
        /// </summary>
        private readonly IShoppingOnlineService ShoppingOrderService;


        /// <summary>
        /// Gets a necessary general data for the operation of the web application and gets a personal data of a customer,
        /// for example: selected products, a selected delivery method, an address and etc.
        /// </summary>
        /// <param name="chatInfo"></param>
        /// <returns></returns>
        [HttpPost("mainInfoForWebAppOrder")]
        public async Task<ActionResult<MainInfoForWebAppOrderOfServerSide>> GeneralInfoForWorkingWithOrderInWebAppAsync(ChatInfo chatInfo)
        {
            try
            {
                // Gets a personal data of a customer via API server
                var orderInfoOfCustomer = await ShoppingOrderService.GetPersonalDataOfOrderAsync(chatInfo);
                // Returns the gotten personal data of a customer, the general data for the operation of the web app and the web app operation mode
                return Ok(new MainInfoForWebAppOrderOfServerSide(orderInfoOfCustomer, ShoppingOrderService.GeneralInfoOfOnlineStore, ShoppingOrderService.IsReleaseMode));
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
        public async Task<ActionResult<WalletBalance>> WalletBalanceOfCustomerAsync(ChatInfo chatInfo)
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
        public async Task<IActionResult> SavePersonalDataOfOrderInServerAsync(PersonalInfoOfOrderByServerSide order)
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
        public async Task<ActionResult<LoyaltyCheckinInfo>> CalculateCheckinAsync(PersonalInfoOfOrderByServerSide order)
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
        public async Task<ActionResult<InvoiceLinkStatus>> RetrieveInvoiceLinkAsync(PersonalInfoOfOrderByServerSide order)
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
