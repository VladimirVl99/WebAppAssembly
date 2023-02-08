using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebAppAssembly.Shared.Entities.Telegram;

namespace WebAppAssembly.Shared.Models.OrderData
{
    public class SavedData
    {
        private string _deliveryMethodAsString;
        private DeliveryMethodType _deliveryMethod;

        /// <summary>
        /// The delivery method.
        /// </summary>
        [JsonProperty("orderServiceType")]
        [JsonPropertyName("orderServiceType")]
        private string? DeliveryMethodAsString
        {
            get => _deliveryMethodAsString;
            set
            {
                _deliveryMethodAsString = value ?? "ByCourier";
                if (!Enum.TryParse(DeliveryMethodAsString, out _deliveryMethod))
                {
                    throw new Exception($"Exception: orderServiceType cannot be equal to {value}.");
                }
            }
        }

        public DeliveryMethodType DeliveryMethod
        {
            get => _deliveryMethod;
            private set
            {
                _deliveryMethod = value;
                DeliveryMethodAsString = value.ToString();
            }
        }

        /// <summary>
        /// A pickup terminal ID.
        /// </summary>
        [JsonProperty("terminalId")]
        [JsonPropertyName("terminalId")]
        public Guid? TerminalId { get; private set; }

        /// <summary>
        /// Additional information about a pickup terminal ID.
        /// </summary>
        [JsonProperty("deliveryTerminal")]
        [JsonPropertyName("deliveryTerminal")]
        public DeliveryTerminal? DeliveryTerminal { get; private set; }

        /// <summary>
        /// A client's address.
        /// </summary>
        [JsonProperty("address")]
        [JsonPropertyName("address")]
        public DeliveryPoint? Address { get; set; }


        public SavedData()
        {
            _deliveryMethodAsString = string.Empty;
        }


        public enum DeliveryMethodType
        {
            DeliveryByCourier,
            DeliveryByClient
        }


        /// <summary>
        /// Sets/changes the delivery terminal of pickup.
        /// </summary>
        /// <param name="terminal"></param>
        public void SetDeliveryTerminal(DeliveryTerminal terminal)
        {
            TerminalId = terminal.Id;
            DeliveryTerminal = terminal;
        }

        public void SetDeliveryMethod(DeliveryMethodType deliveryMethod)
            => DeliveryMethod = deliveryMethod;
    }
}