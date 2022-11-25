#region
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

namespace WebAppAssembly.Server.Repositories.OrderCreationOrderInWebRepository
{
    public class WebOrderService
    {
        public WebOrderService(IConfiguration configuration)
        {
            Console.WriteLine(typeof(WebOrderService).FullName!, nameof(WebOrderService), $"Initializing the {nameof(OrderModel)} object and receiving the product items via db");
            Configuration = configuration;
            IsTestMode = Convert.ToBoolean(configuration["WebAppMode:TestMode"]);
            Url = new ApiServerUrls(configuration);

            try
            {
                var webAppInfoTask = WebAppInfoAsync(Url.WebAppInfo);
                webAppInfoTask.Wait();

                WebAppInfo = webAppInfoTask.Result;
                DeliveryTerminals = webAppInfoTask.Result.DeliveryTerminals ?? throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(Exception)}: " +
                        $"{nameof(DeliveryTerminals)} can't be null");

                Console.WriteLine(typeof(WebOrderService).FullName!, nameof(WebOrderService), $"The {nameof(OrderModel)} object is initialized and the product items is received");
            }
            catch (Exception ex)
            {
                Console.WriteLine(typeof(WebOrderService).FullName!, nameof(WebOrderService), LogService.FormatExceptionActionContent(ex));
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private readonly IConfiguration Configuration;
        public OrderClientModel? OrderModel { get; set; }
        public IEnumerable<DeliveryTerminal> DeliveryTerminals { get; private set; }
        public double DiscountBalance =>
            discountBalance == null ? GetCustomerBalanceAsync(Configuration["TelegramBotProperties:customerBalance"]).Result : (double)discountBalance;

        private double? discountBalance = null;
        public bool IsTestMode { get; }
        public long ChatId { get; set; } = 0;
        private ApiServerUrls Url { get; set; }
        public WebAppInfo WebAppInfo { get; set; }

        /// <summary>
        /// Initialize all order parameters
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public async Task<OrderClientModel?> InitializeOrderModelAsync()
        {
            try
            {
                var obj = await OrderModelCashAsync(Url.OrderModel, ChatId);
                return OrderModel = obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Receive basic info from server
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        /// <exception cref="Exception"></exception>
        private static async Task<WebAppInfo> WebAppInfoAsync(string url)
        {
            using var httpClient = new HttpClient();
            try
            {
                var responseTask = httpClient.GetAsync(url);
                responseTask.Wait();
                var response = responseTask.Result;
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                    throw new HttpProcessException(response.StatusCode, responseBody);

                return JsonConvert.DeserializeObject<WebAppInfo>(responseBody) ?? throw new Exception("Json convert web app info is null");
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                    throw e;
                throw ae;
            }
        }

        /// <summary>
        /// Receive order parameters
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        /// <exception cref="Exception"></exception>
        private static async Task<OrderClientModel?> OrderModelCashAsync(string url, long chatId)
        {
            using var httpClient = new HttpClient();
            try
            {
                var body = JsonConvert.SerializeObject(new { chatId });
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, data);

                string responseBody = await response.Content.ReadAsStringAsync();
                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                    throw new HttpProcessException(response.StatusCode, responseBody);

                return JsonConvert.DeserializeObject<OrderClientModel>(responseBody) ?? throw new Exception("Json convert order model is empty");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                    return null;
                throw ex;
            }
            catch (HttpProcessException ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                    return null;
                throw ex;
            }
        }

        /// <summary>
        /// Get customer's balance
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<double> GetCustomerBalanceAsync(string url)
        {
            if (IsTestMode)
                ChatId = 2098619539;

            using var httpClient = new HttpClient();
            try
            {
                string body = JsonConvert.SerializeObject(new { chatId = ChatId });
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                var responseTask = httpClient.PostAsync(url, data);
                responseTask.Wait();
                var response = responseTask.Result;
                response.EnsureSuccessStatusCode();

                // Get body from the response
                string responseBody = await response.Content.ReadAsStringAsync();
                if (!response.StatusCode.Equals(HttpStatusCode.OK))               
                    return (double)(discountBalance = 0);

                var customer_discount_balance = JsonConvert.DeserializeObject<CustomerDiscountBalance>(responseBody)
                    ?? throw new Exception($"{nameof(GetCustomerBalanceAsync)}: This object can't be null");

                return (double)(discountBalance = customer_discount_balance.Balance);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return (double)(discountBalance = 0);
            }
        }

        /// <summary>
        /// Update total sum of order
        /// </summary>
        /// <exception cref="Exception"></exception>
        public double UpdateTotalSumOfOrder()
        {
            double total = 0;
            if (OrderModel is null) throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(UpdateTotalSumOfOrder)}.{nameof(Exception)}: " +
                $"{nameof(OrderModel)} can't be null");

            var items = OrderModel.Items;
            if (items is not null)
            {
                foreach (var item in items)
                {
                    var product = WebAppInfo.TransportItemDtos?.FirstOrDefault(x => x.ItemId == item.ProductId)
                    ?? throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(UpdateTotalSumOfOrder)}.{nameof(Exception)}: " +
                        $"{nameof(Product)} can't be null");

                    if (item.HaveModifiers()) total += TotalSumOfSelectedProductWithModifiers(product, item);
                    else total += item.Amount * product.ItemSizes?.LastOrDefault()?.Prices?.LastOrDefault()?.Price ?? throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(UpdateTotalSumOfOrder)}.{nameof(Exception)}: " +
                        $"Price of product can't be null");
                }
            }
            else total = double.MaxValue;
            return OrderModel.TotalSum = total;
        }

        /// <summary>
        /// Get parameters of products by group ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<TransportItemDto>? GetListOfProductsByGroupId(Guid id)
        {
            var productsByGroup = WebAppInfo.ItemCategories?.FirstOrDefault(x => x.Id == id)?.Items;
            return productsByGroup;
        }

        /// <summary>
        /// Add one position to product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <exception cref="Exception"></exception>
        public void AddProduct(Guid productId, Guid? positionId = null)
        {
            Console.WriteLine(typeof(WebOrderService).FullName!, nameof(AddProduct), "Increase an amount of product");
            if (OrderModel is null) throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(AddProduct)}.{nameof(Exception)}: " +
                $"{nameof(OrderModel)} can't be null");

            if (OrderModel.Items is not null)
            {
                if (positionId != null && positionId != Guid.Empty)
                    OrderModel.Items.Last(x => x.ProductId == productId && x.PositionId == positionId).IncrementAmountWithPrice();
                else
                    OrderModel.Items.First(x => x.ProductId == productId).IncrementAmountWithPrice();
                OrderModel.IncrementTotalAmount();

                var product = WebAppInfo.TransportItemDtos?.FirstOrDefault(x => x.ItemId == productId);
                if (product is not null)
                    product.IncrementAmount();
            }
        }

        /// <summary>
        /// Add one position to product
        /// </summary>
        /// <param name="product"></param>
        /// <param name="item"></param>
        public void AddProduct(ref Product product, ref Item item)
        {
            Console.WriteLine(typeof(WebOrderService).FullName!, nameof(AddProduct), "Increase an amount of product");
            if (OrderModel is null) throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(AddProduct)}.{nameof(Exception)}: " +
                $"{nameof(OrderModel)} can't be null");

            item.IncrementAmountWithPrice();
            OrderModel.IncrementTotalAmount();
            product.IncrementAmount();
        }

        /// <summary>
        /// Subtract one position from product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <param name="product"></param>
        public void RemoveProduct(Guid productId, Guid? positionId = default)
        {
            Console.WriteLine(typeof(WebOrderService).FullName!, nameof(RemoveProduct), "Decrease an amount of product");
            if (OrderModel is null) throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(RemoveProduct)}.{nameof(Exception)}: " +
                $"{nameof(OrderModel)} can't be null");

            if (OrderModel.Items is not null)
            {
                if (positionId != default && positionId != Guid.Empty)
                    OrderModel.Items.First(x => x.ProductId == productId && x.PositionId == positionId).DecrementAmountWithPrice();
                else
                    OrderModel.Items.First(x => x.ProductId == productId).DecrementAmountWithPrice();
                OrderModel.DecrementTotalAmount();

                var product = WebAppInfo.TransportItemDtos?.FirstOrDefault(x => x.ItemId == productId);
                if (product != null) product.DecrementAmount();
            }
        }

        /// <summary>
        /// Subtract one position from product
        /// </summary>
        /// <param name="product"></param>
        /// <param name="item"></param>
        public void RemoveProduct(ref Product product, ref Item item)
        {
            Console.WriteLine(typeof(WebOrderService).FullName!, nameof(RemoveProduct), "Decrease an amount of product");
            if (OrderModel is null) throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(RemoveProduct)}.{nameof(Exception)}: " +
                $"{nameof(OrderModel)} can't be null");

            item.DecrementAmountWithPrice();
            OrderModel.DecrementTotalAmount();
            product.DecrementAmount();
        }

        /// <summary>
        /// Add one position to modifier by id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="modifierId"></param>
        /// <param name="positionId"></param>
        /// <param name="productGroupId"></param>
        /// <exception cref="Exception"></exception>
        public void AddModifier(Guid productId, Guid modifierId, Guid positionId, Guid? productGroupId = default)
        {
            Console.WriteLine(typeof(WebOrderService).FullName!, nameof(AddModifier), "Increase an amount of modifier");
            if (OrderModel is null) throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(AddModifier)}.{nameof(Exception)}: " +
                $"{nameof(OrderModel)} can't be null");

            if (OrderModel.Items is not null)
                OrderModel.Items.First(x => x.ProductId == productId && x.PositionId == positionId).IncreaseAmountOfModifier(modifierId, productGroupId);
        }

        /// <summary>
        /// Add one position to modifier by id
        /// </summary>
        /// <param name="product"></param>
        /// <param name="item"></param>
        /// <param name="modifierId"></param>
        /// <param name="productGroupId"></param>
        public void AddModifier(ref Item item, Guid modifierId, Guid? productGroupId = null)
        {
            Console.WriteLine(typeof(WebOrderService).FullName!, nameof(AddModifier), "Increase an amount of modifier");
            item.IncreaseAmountOfModifier(modifierId, productGroupId);
        }

        /// <summary>
        /// Subtract one position from modifier by id
        /// </summary>
        /// <param name="item"></param>
        /// <param name="modifierId"></param>
        /// <param name="productGroupId"></param>
        public void RemoveModifier(ref Item item, Guid modifierId, Guid? productGroupId = null)
        {
            Console.WriteLine(typeof(WebOrderService).FullName!, nameof(RemoveModifier), "Decrease an amount of modifier");
            item.DecreaseAmountOfModifier(modifierId, productGroupId);
        }

        /// <summary>
        /// Subtract one position from modifier by id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="modifierId"></param>
        /// <param name="positionId"></param>
        /// <param name="productGroupId"></param>
        public void RemoveModifier(Guid productId, Guid modifierId, Guid positionId, Guid? productGroupId = default)
        {
            Console.WriteLine(typeof(WebOrderService).FullName!, nameof(RemoveModifier), "Decrease an amount of modifier");
            if (OrderModel is null) throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(RemoveModifier)}.{nameof(Exception)}: " +
                $"{nameof(OrderModel)} can't be null");

            if (OrderModel.Items is not null)
                OrderModel.Items.First(x => x.ProductId == productId && x.PositionId == positionId).DecreaseAmountOfModifier(modifierId, productGroupId);
        }

        /// <summary>
        /// Total sum of the selected product with modifiers
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public double TotalSumOfSelectedProductWithModifiers(Guid productId, Guid positionId)
        {
            if (OrderModel is null) throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(TotalSumOfSelectedProductWithModifiers)}.{nameof(Exception)}: " +
                $"{nameof(OrderModel)} can't be null");

            var product = WebAppInfo.TransportItemDtos?.FirstOrDefault(x => x.ItemId == productId);
            if (OrderModel.Items is not null)
            {
                var item = OrderModel.Items.First(x => x.ProductId == productId && x.PositionId == positionId);

                if (product != null)
                {
                    var productPrice = (item.ProductSizeId is not null ? product.Price((Guid)item.ProductSizeId) : product.Price())
                        ?? throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(TotalSumOfSelectedProductWithModifiers)}.{nameof(Exception)}: " +
                        $"Price of product by ID - '{product.ItemId}' can't be null");
                    return (productPrice + PriceWithModifiersByProductId(item)) * item.Amount;
                }
                throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(TotalSumOfSelectedProductWithModifiers)}.{nameof(Exception)}: " +
                    $"Product by ID - '{productId}' can't be null");
            }
            return double.MaxValue;
        }

        /// <summary>
        /// Total sum of the selected product with modifiers
        /// </summary>
        /// <param name="product"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public double TotalSumOfSelectedProductWithModifiers(TransportItemDto product, Item item)
        {
            var productPrice = product.ItemSizes?.LastOrDefault()?.Prices?.LastOrDefault()?.Price
                ?? throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(TotalSumOfSelectedProductWithModifiers)}.{nameof(Exception)}: " +
                $"Price of product by ID - '{product.ItemId}' can't be null");
            return (productPrice + PriceWithModifiersByProductId(item)) * item.Amount;
        }

        /// <summary>
        /// Add new item to order with position id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public Guid AddItemToOrderWithNewPosition(Guid productId, TransportItemDto? product = null)
        {
            if (OrderModel is null) throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(AddItemToOrderWithNewPosition)}.{nameof(Exception)}: " +
                $"{nameof(OrderModel)} can't be null");

            var newId = Guid.NewGuid();
            product ??= WebAppInfo.TransportItemDtos?.FirstOrDefault(x => x.ItemId == productId);
            if (product is not null && OrderModel.Items is not null)
            {
                OrderModel.Items.Add(new Item(product,  newId));
            }
            return newId; // ???
        }

        /// <summary>
        /// Add new item to order
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="product"></param>
        public void AddItemToOrder(Guid productId, TransportItemDto? product = null)
        {
            if (OrderModel is null) throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(AddItemToOrder)}.{nameof(Exception)}: " +
                $"{nameof(OrderClientModel)} can't be null");

            product ??= WebAppInfo.TransportItemDtos?.FirstOrDefault(x => x.ItemId == productId);
            if (product is not null && OrderModel.Items is not null)
            {
                OrderModel.Items.Add(new Item(product));
            }
        }

        /// <summary>
        /// Add a comment to the order
        /// </summary>
        /// <param name="comment"></param>
        /// <exception cref="Exception"></exception>
        public void AddComment(string comment)
        {
            Console.WriteLine(typeof(WebOrderService).FullName!, nameof(AddComment), "Add a comment");
            if (OrderModel is null) throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(AddComment)}.{nameof(Exception)}: " +
                $"{nameof(OrderModel)} can't be null");

            OrderModel.Comment = comment;
        }

        /// <summary>
        /// Add delivery point
        /// </summary>
        /// <param name="city"></param>
        /// <param name="street"></param>
        /// <param name="house"></param>
        /// <param name="flat"></param>
        /// <param name="entrance"></param>
        /// <param name="floor"></param>
        public void AddDeliveryPoint(string city, string street, string house, string? flat = null, string? entrance = null, string? floor = null)
        {
            if (OrderModel is null) throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(AddDeliveryPoint)}.{nameof(Exception)}: " +
                $"{nameof(OrderModel)} can't be null");
            OrderModel.Address = new(city, street, house, flat, entrance, floor);
        }

        /// <summary>
        /// Add received chatId via the TelegramBot to the order
        /// </summary>
        /// <param name="chatId"></param>
        public void AddChatId(long chatId)
        {
            Console.WriteLine(typeof(WebOrderService).FullName!, nameof(AddChatId), "Add a chat id via the TelegramBot");
            ChatId = chatId;
            if (OrderModel != null) OrderModel.ChatId = chatId;
        }

        /// <summary>
        /// Info about the created order and save items of order in the database
        /// </summary>
        /// <returns></returns>
        public string InfoAboutCreatedOrder()
        {
            Console.WriteLine(typeof(WebOrderService).FullName!, nameof(InfoAboutCreatedOrder),
                $"Insert the {nameof(OrderModel)} item into db and info in the WebApp about the created order");
            if (OrderModel is null) throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(InfoAboutCreatedOrder)}.{nameof(Exception)}: " +
                $"{nameof(OrderModel)} can't be null");

            try
            {
                // Info of the selected products
                string selectedProductsInfo = string.Empty;
                if (OrderModel.Items is not null)
                    foreach (var product in OrderModel.Items)
                        selectedProductsInfo += $"{product.ProductName} x{product.Amount} - ₽{product.Price}\n";

                return $"Order summary:\n" +
                $"operationId: {OrderModel.OperationId}\n" +
                $"\n{selectedProductsInfo}\n" +
                $"Total: ₽{OrderModel.TotalSum}\n" +
                $"Comment: {OrderModel.Comment}\n" +
                $"Order's create date: {OrderModel.CreatedDate}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(typeof(WebOrderService).FullName!, nameof(InfoAboutCreatedOrder), LogService.FormatExceptionActionContent(ex));
                return "Database Error";
            }
        }

        /// <summary>
        /// Send information about the created order to the TelegramBot
        /// </summary>
        /// <returns></returns>
        public async Task SendCreatedOrderToTelegramBot()
        {
            if (OrderModel is null) throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(SendCreatedOrderToTelegramBot)}.{nameof(Exception)}: " +
                $"{nameof(OrderModel)} can't be null");

            if (OrderModel.Items is not null)
            {
                foreach (var item in OrderModel.Items)
                {
                    if (item.Amount != 0)
                    {
                        if (item.HaveModifiers())
                        {
                            item.Price = TotalSumOfSelectedProductWithModifiers(item.ProductId, item.PositionId ?? Guid.Empty);
                        }
                        else
                        {
                            item.Price = item.Amount * PriceByProductId(item.ProductId);
                        }
                    }
                }
            }
            Console.WriteLine(typeof(WebOrderService).FullName!, nameof(SendCreatedOrderToTelegramBot),
                $"Send an operation id of the created order to server");

            //HttpClientHandler clientHandler = new HttpClientHandler();
            //clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var client = new HttpClient();
            try
            {
                var body = JsonConvert.SerializeObject(OrderModel);
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Configuration["TelegramBotProperties:sendOrderUrl"], data);
                string result = await response.Content.ReadAsStringAsync();
                Console.WriteLine(typeof(WebOrderService).FullName!, nameof(SendCreatedOrderToTelegramBot),
                    $"The operation id of the created order has been sended to server");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(typeof(WebOrderService).FullName!, nameof(SendCreatedOrderToTelegramBot), LogService.FormatExceptionActionContent(ex));
                Console.WriteLine(typeof(WebOrderService).FullName!, nameof(SendCreatedOrderToTelegramBot),
                    $"The operation id of the created order hasn't been sended to server");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(typeof(WebOrderService).FullName!, nameof(SendCreatedOrderToTelegramBot), LogService.FormatExceptionActionContent(ex));
            }
        }

        /// <summary>
        /// Price of the product by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public double PriceByProductId(Guid id, Guid? productSizeId = null)
        {
            var product = WebAppInfo.TransportItemDtos?.FirstOrDefault(x => x.ItemId == id);
            return product?.Price(productSizeId) ?? throw new Exception($"{typeof(WebOrderService).FullName}." +
                $"{nameof(PriceByProductId)}.{nameof(Exception)}: {nameof(WebAppInfo.TransportItemDtos)} can't be null");
        }

        /// <summary>
        /// Price of the product with modifiers by ID
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public double PriceWithModifiersByProductId(Guid productId, Guid positionId)
        {
            if (OrderModel is null) throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(PriceWithModifiersByProductId)}.{nameof(Exception)}: " +
                $"{nameof(OrderModel)} can't be null");

            if (OrderModel.Items is not null)
            {
                var modifiers = OrderModel.Items.First(x => x.ProductId == productId && x.PositionId == positionId).SelectedModifiers();
                double total = 0;
                foreach (var modifier in modifiers)
                    total += PriceByProductId(modifier.ProductId) * modifier.Amount;
                return total;
            }
            else return double.MaxValue;
        }

        /// <summary>
        /// Price of the product with modifiers
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public double PriceWithModifiersByProductId(Item item)
        {
            var modifiers = item.SelectedModifiers();
            double total = 0;
            foreach (var modifier in modifiers)
                total += modifier.Price ?? throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(PriceWithModifiersByProductId)}.{nameof(Exception)}: " +
                    $"Price of modifier by ID - '{modifier.ProductId}' can't be null");
            return total;
        }

        /// <summary>
        /// Image link of the product by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ImageLinkByProductId(Guid id)
        {
            var imageLink = WebAppInfo.TransportItemDtos?.FirstOrDefault(x => x.ItemId == id)?.ImageLink();
            return imageLink ?? string.Empty;
        }

        /// <summary>
        /// Send the order data to server
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        public async Task SendChangedOrderModelToServerAsync()
        {
            using var httpClient = new HttpClient();
            try
            {
                var body = JsonConvert.SerializeObject(new { chatId = ChatId, orderInfoViaWebApp = OrderModel });
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

        /// <summary>
        /// Process total sum of order
        /// </summary>
        /// <returns></returns>
        public void OrderModelForInvoice(ref OrderClientModel order)
        {
            if (order.Items is not null)
            {
                foreach (var item in order.Items)
                {
                    if (item.Amount > 0)
                    {
                        if (item.HaveModifiers())
                        {
                            item.Modifiers = item.Modifiers?.Where(x => x.Amount > 0);
                            if (item.Modifiers is not null)
                            {
                                foreach (var modifier in item.Modifiers)
                                {
                                    modifier.ProductGroupId = modifier.ProductGroupId == Guid.Empty ? null : modifier.ProductGroupId;
                                }
                            }

                            item.Price = TotalSumOfSelectedProductWithModifiers(item.ProductId, item.PositionId ?? Guid.Empty);
                        }
                        else item.Price = item.Amount * PriceByProductId(item.ProductId, item.ProductSizeId);
                    }
                }
            }
        }

        /// Create invoice link in the server
        /// </summary>
        /// <returns></returns>
        public async Task<InvoiceLinkStatus> CreateInvoiceLinkAsync()
        {
            if (OrderModel is null) throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(CalculateCheckinAsync)}.{nameof(Exception)}: " +
                $"{nameof(OrderModel)} can't be null");
            var orderModel = (OrderClientModel)OrderModel.Clone();
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
            catch (HttpRequestException hre)
            {
                Console.WriteLine(hre.Message);
                return new(false, null, hre.Message);
            }
            catch (HttpProcessException hpe)
            {
                Console.WriteLine(hpe.Message);
                return new(false, null, hpe.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new(false, null, ex.Message);
            }
        }

        /// <summary>
        /// Calculate checkin
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpProcessException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<Checkin> CalculateCheckinAsync()
        {
            if (OrderModel is null) throw new Exception($"{typeof(WebOrderService).FullName}.{nameof(CalculateCheckinAsync)}.{nameof(Exception)}: " +
                $"{nameof(OrderModel)} can't be null");

            string responseBody = string.Empty;
            using (var client = new HttpClient())
            {
                var body = JsonConvert.SerializeObject(OrderModel);
                var data = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Url.Checkin, data);

                responseBody = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new HttpProcessException(response.StatusCode, responseBody);
            }

            return JsonConvert.DeserializeObject<Checkin>(responseBody) ?? throw new Exception("Received checkin result is empty");
        }

        /// <summary>
        /// Receive the wallet balance by chat ID
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="HttpProcessException"></exception>
        public async Task<WalletBalance> WalletBalanceAsync()
        {
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

            return JsonConvert.DeserializeObject<WalletBalance>(responseBody) ?? throw new Exception("Received the wallet balance result is empty");
        }
    }
}
#endregion