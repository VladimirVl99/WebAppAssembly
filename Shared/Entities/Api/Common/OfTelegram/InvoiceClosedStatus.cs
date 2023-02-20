namespace WebAppAssembly.Shared.Entities.Api.Common.OfTelegram
{
    /// <summary>
    /// Invoice statuses:
    /// - paid – invoice was paid successfully,
    /// - cancelled – user closed this invoice without paying,
    /// - failed – user tried to pay, but the payment was failed,
    /// - pending – the payment is still processing. The bot will receive
    /// a service message about a successful payment when the payment is successfully paid.
    /// </summary>
    public enum InvoiceClosedStatus
    {
        Paid,
        Cancelled,
        Failed,
        Pending,
        Error
    }
}
