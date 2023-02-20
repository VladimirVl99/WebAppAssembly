using Newtonsoft.Json;
using SourceInvoiceLinkStatus = WebAppAssembly.Shared.Entities.Api.Common.OfTelegram.InvoiceLinkStatus;

namespace WebAppAssembly.Shared.Entities.Api.Requests.OfTelegram
{
    /// <summary>
    /// For an initialization.
    /// Information about an invoice link status that is received from the Telegram.
    /// Source: https://core.telegram.org/bots/api#createinvoicelink.
    /// </summary>
    [JsonObject]
    public class InvoiceLinkStatus : SourceInvoiceLinkStatus
    {
        public InvoiceLinkStatus(bool ok, string? invoiceLink = null,
            string? message = null, string? exMessage = null)
            : base()
        {
            Ok = ok;
            InvoiceLink = invoiceLink;
            Message = message;
            ExMessage = exMessage;
        }
    }
}
