using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.OfTelegram
{
    /// <summary>
    /// Information about an invoice link status that is received from the Telegram.
    /// Source: https://core.telegram.org/bots/api#createinvoicelink.
    /// </summary>
    [JsonObject]
    public class InvoiceLinkStatus
    {
        /// <summary>
        /// Status.
        /// </summary>
        [JsonProperty(PropertyName = "ok", Required = Required.Always)]
        public bool Ok { get; set; }

        /// <summary>
        /// If 'Ok' property has the value true, then a link for an invoice will be contained.
        /// </summary>
        [JsonProperty(PropertyName = "invoiceLink", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? InvoiceLink { get; set; }

        /// <summary>
        /// If 'Ok' property has the value false, then this property will contain on a specified error message.
        /// </summary>
        [JsonProperty(PropertyName = "message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Message { get; set; }

        /// <summary>
        /// If 'Ok' property has the value false, then this property will contain on a exception message.
        /// </summary>
        [JsonProperty(PropertyName = "exMessage", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ExMessage { get; set; }
    }
}