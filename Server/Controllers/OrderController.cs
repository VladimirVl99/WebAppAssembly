using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAppAssembly.Server.Repositories.OrderCreationOrderInWebRepository;
using WebAppAssembly.Shared.Entities;
using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.Models.Order;

namespace WebAppAssembly.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly WebOrderService _orderService;
        private readonly IConfiguration _configuration;

        public OrderController(ILogger<OrderController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _orderService = new WebOrderService(configuration);
        }

        [HttpGet(nameof(FoodShopInfo))]
        public ActionResult<FoodShopInfo> GetFoodShopAsync()
        {
            try
            {
                return Ok(new FoodShopInfo(_orderService.DeliveryTerminals, _orderService.IsTestMode,
                    _orderService.WebAppInfo));         
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost(nameof(OrderClientModel))]
        public async Task<ActionResult<OrderClientModel?>> GetOrderModelAsync(ChatInfo chatInfo)
        {
            try
            {
                _orderService.ChatId = chatInfo.ChatId;
                return Ok(await _orderService.InitializeOrderModelAsync());
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
                _orderService.ChatId = chatInfo.ChatId;
                return Ok(await _orderService.WalletBalanceAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("saveChangedOrder")]
        public async Task<IActionResult> SaveChangedOrderAsync(OrderClientModel order)
        {
            try
            {
                if (_orderService.ChatId == 0) _orderService.AddChatId(order.ChatId);
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

        [HttpPost("calculateCheckin")]
        public async Task<ActionResult<Checkin>> CalculateCheckinAsync(OrderClientModel order)
        {
            try
            {
                if (_orderService.ChatId == 0) _orderService.AddChatId(order.ChatId);
                _orderService.OrderModel ??= order;                
                return Ok(await _orderService.CalculateCheckinAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("createInvoiceLink")]
        public async Task<ActionResult<InvoiceLinkStatus>> CreateInvoiceLinkAsync(OrderClientModel order)
        {
            try
            {
                if (_orderService.ChatId == 0) _orderService.AddChatId(order.ChatId);
                _orderService.OrderModel ??= order;
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
