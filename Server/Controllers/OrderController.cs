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
                return Ok(new FoodShopInfo(_orderService.DeliveryTerminals, _orderService.WebAppMenu, _orderService.IsTestMode,
                    _orderService.WebAppInfo));         
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost(nameof(OrderModel))]
        public async Task<ActionResult<FoodShopInfo>> GetOrderModelAsync(ChatInfo chatInfo)
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

        [HttpPost("saveChangedOrder")]
        public async Task<IActionResult> SaveChangedOrderAsync(OrderModel order)
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
    }
}
