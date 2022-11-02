namespace WebAppAssembly.Shared.Entities.IikoCloudApi
{
    public class Discount
    {
        /// <summary>
        /// Enum: 0 1 2 3
        /// Operation Type Code.
        /// 0 - fixed discount for the entire order,
        /// 1 - fixed discount for the item,
        /// 2 - free product,
        /// 3 - other type of discounts
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// Id of item the discount is applied to. If null - discount applied to whole orders
        /// </summary>
        public Guid? OrderItemId { get; set; }
        /// <summary>
        /// Discount sum
        /// </summary>
        public double DiscountSum { get; set; }
        /// <summary>
        /// Amount
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// Comment. Can be null
        /// </summary>
        public string? Comment { get; set; }
    }
}
