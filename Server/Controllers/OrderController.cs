using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAppAssembly.Server.Repositories.OrderCreationOrderInWebRepository;
using WebAppAssembly.Shared.Entities.OfServerSide;
using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.Entities.WebApp;
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
            _configuration = configuration;
            _orderService = new WebOrderService(configuration);
        }

        private readonly ILogger<OrderController> _logger;
        private readonly WebOrderService _orderService;
        private readonly IConfiguration _configuration;


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
                await _orderService.InitializeOrderModelAsync(chatInfo.ChatId);
                return Ok(new MainInfoForWebAppOrderOfServerSide(_orderService.OrderModel, _orderService.WebAppInfo, _orderService.IsReleaseMode,
                    _orderService.TlgMainBtnColor));
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
                return Ok(await _orderService.WalletBalanceAsync());
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
        public async Task<IActionResult> SaveChangedOrderAsync(OrderClientModel order)
        {
            try
            {
                _orderService.OrderModel = order;
                await _orderService.SendChangedOrderModelToServerAsync();
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
        public async Task<ActionResult<LoyaltyCheckinInfo>> CalculateCheckinAsync(OrderClientModel order)
        {
            try
            {
                _orderService.OrderModel = order;
                return Ok(await _orderService.CalculateCheckinAsync());
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
        public async Task<ActionResult<InvoiceLinkStatus>> CreateInvoiceLinkAsync(OrderClientModel order)
        {
            try
            {
                _orderService.OrderModel = order;
                return Ok(await _orderService.CreateInvoiceLinkAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
