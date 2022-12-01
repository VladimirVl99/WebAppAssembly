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
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        public OrderController(ILogger<OrderController> logger, IConfiguration configuration)
        {
            _logger = logger;
            ShoppingOrderService = new ShoppingOnlineService(configuration);
        }

        private readonly ILogger<OrderController> _logger;
        private readonly IShoppingOnlineService ShoppingOrderService;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatInfo"></param>
        /// <returns></returns>
        [HttpPost("mainInfoForWebAppOrder")]
        public async Task<ActionResult<MainInfoForWebAppOrderOfServerSide>> MainInfoForWebAppOrderAsync(ChatInfo chatInfo)
        {
            try
            {
                var orderInfoOfCustomer = await ShoppingOrderService.GetOrderModelCashAsync(chatInfo.ChatId);
                return Ok(new MainInfoForWebAppOrderOfServerSide(orderInfoOfCustomer, ShoppingOrderService.DeliveryGeneralInfo, ShoppingOrderService.IsReleaseMode));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Receive the wallet balance by chat ID
        /// </summary>
        /// <param name="chatInfo"></param>
        /// <returns></returns>
        [HttpPost("walletBalance")]
        public async Task<ActionResult<WalletBalance>> WalletBalanceAsync(ChatInfo chatInfo)
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
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost("saveOrderInfoInServer")]
        public async Task<IActionResult> SendOrderInfoToServerAsync(OrderModelOfServer order)
        {
            try
            {
                await ShoppingOrderService.SendOrderInfoToServerAsync(order);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost("calculateCheckin")]
        public async Task<ActionResult<LoyaltyCheckinInfo>> CalculateCheckinAsync(OrderModelOfServer order)
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
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost("createInvoiceLink")]
        public async Task<ActionResult<InvoiceLinkStatus>> CreateInvoiceLinkAsync(OrderModelOfServer order)
        {
            try
            {
                return Ok(await ShoppingOrderService.CreateInvoiceLinkAsync(order));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
