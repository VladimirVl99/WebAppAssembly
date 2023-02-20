namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.Loyalties
{
    /// <summary>
    /// Operation Type Code.
    /// 0 - fixed discount for the entire order,
    /// 1 - fixed discount for the item,
    /// 2 - free product,
    /// 3 - other type of discounts.
    /// </summary>
    public enum OperationTypeCode
    {
        FixedDiscountForEntireOrder,
        FixedDiscountForItem,
        FreeProduct,
        OtherTypeOfDiscounts
    }
}
