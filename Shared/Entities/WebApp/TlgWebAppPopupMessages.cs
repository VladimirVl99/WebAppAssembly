using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Entities.WebApp
{
    public class TlgWebAppPopupMessages
    {
        [JsonProperty("loayltyProgramUnavailable")]
        [JsonPropertyName("loayltyProgramUnavailable")]
        public TlgWebAppPopupMessage? LoayltyProgramUnavailable { get; set; }
        [JsonProperty("unavailableMinSumWithDiscountForPay")]
        [JsonPropertyName("unavailableMinSumWithDiscountForPay")]
        public TlgWebAppPopupMessage? UnavailableMinSumWithDiscountForPay { get; set; }
        [JsonProperty("unavailableMinSumtForPay")]
        [JsonPropertyName("unavailableMinSumtForPay")]
        public TlgWebAppPopupMessage? UnavailableMinSumtForPay { get; set; }
        [JsonProperty("incorrectSelectingOfModifier")]
        [JsonPropertyName("incorrectSelectingOfModifier")]
        public TlgWebAppPopupMessage? IncorrectSelectedModifier { get; set; }
        [JsonProperty("incorrectCityFormat")]
        [JsonPropertyName("incorrectCityFormat")]
        public TlgWebAppPopupMessage? IncorrectCityFormat { get; set; }
        [JsonProperty("incorrectStreetFormat")]
        [JsonPropertyName("incorrectStreetFormat")]
        public TlgWebAppPopupMessage? IncorrectStreetFormat { get; set; }
        [JsonProperty("incorrectHouseFormat")]
        [JsonPropertyName("incorrectHouseFormat")]
        public TlgWebAppPopupMessage? IncorrectHouseFormat { get; set; }
        [JsonProperty("walletBalanceChangedInIikoBiz")]
        [JsonPropertyName("walletBalanceChangedInIikoBiz")]
        public TlgWebAppPopupMessage? WalletBalanceChangedInIikoBiz { get; set; }
        [JsonProperty("emptyShoppingCart")]
        [JsonPropertyName("emptyShoppingCart")]
        public TlgWebAppPopupMessage? EmptyShoppingCart { get; set; }
        [JsonProperty("emptySelectedItemsWithModifiers")]
        [JsonPropertyName("emptySelectedItemsWithModifiers")]
        public TlgWebAppPopupMessage? EmptySelectedItemsWithModifiers { get; set; }
    }
}
