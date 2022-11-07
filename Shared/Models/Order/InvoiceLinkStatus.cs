using Newtonsoft.Json;
using System.Text.Json.Serialization;

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

        [JsonProperty("ok")]
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }
        [JsonProperty("invoiceLink")]
        [JsonPropertyName("invoiceLink")]
        public string? InvoiceLink { get; set; }
        [JsonProperty("message")]
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonProperty("exMessage")]
        [JsonPropertyName("exMessage")]
        public string? ExMessage { get; set; }
    }
}
