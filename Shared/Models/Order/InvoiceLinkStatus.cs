namespace WebAppAssembly.Shared.Models.Order
{
    public class InvoiceLinkStatus
    {
        public InvoiceLinkStatus() { }
        public InvoiceLinkStatus(bool ok, string? invoiceLink = null, string? message = null, string? exMessage = null)
        {
            Ok = ok;
            InvoiceLink = invoiceLink;
            Message = message;
            ExMessage = exMessage;
        }

        public bool Ok { get; set; }
        public string? InvoiceLink { get; set; }
        public string? Message { get; set; }
        public string? ExMessage { get; set; }
    }
}
