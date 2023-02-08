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
        [JsonProperty(PropertyName = "loayltyProgramUnavailable", Required = Required.Always)]
        public TlgWebAppPopupMessage LoayltyProgramUnavailable { get; set; } = default!;

        [JsonProperty(PropertyName = "unavailableMinSumWithDiscountForPay", Required = Required.Always)]
        public TlgWebAppPopupMessage UnavailableMinSumWithDiscountForPay { get; set; } = default!;

        [JsonProperty(PropertyName = "unavailableMinSumtForPay", Required = Required.Always)]
        public TlgWebAppPopupMessage UnavailableMinSumtForPay { get; set; } = default!;

        [JsonProperty(PropertyName = "incorrectSelectingOfModifier", Required = Required.Always)]
        public TlgWebAppPopupMessage IncorrectSelectedModifier { get; set; } = default!;

        [JsonProperty(PropertyName = "incorrectCityFormat", Required = Required.Always)]
        public TlgWebAppPopupMessage IncorrectCityFormat { get; set; } = default!;

        [JsonProperty(PropertyName = "incorrectStreetFormat", Required = Required.Always)]
        public TlgWebAppPopupMessage IncorrectStreetFormat { get; set; } = default!;

        [JsonProperty(PropertyName = "incorrectHouseFormat", Required = Required.Always)]
        public TlgWebAppPopupMessage IncorrectHouseFormat { get; set; } = default!;

        [JsonProperty(PropertyName = "walletBalanceChangedInIikoBiz", Required = Required.Always)]
        public TlgWebAppPopupMessage WalletBalanceChangedInIikoBiz { get; set; } = default!;

        [JsonProperty(PropertyName = "emptyShoppingCart", Required = Required.Always)]
        public TlgWebAppPopupMessage EmptyShoppingCart { get; set; } = default!;

        [JsonProperty(PropertyName = "emptySelectedItemsWithModifiers", Required = Required.Always)]
        public TlgWebAppPopupMessage EmptySelectedItemsWithModifiers { get; set; } = default!;
    }
}
