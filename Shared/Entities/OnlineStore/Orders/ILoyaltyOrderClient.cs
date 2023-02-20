using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.OnlineStore.Orders
{
    /// <summary>
    /// For orders that is using loyalty programs.
    /// </summary>
    public interface ILoyaltyOrderClient
    {
        /// <summary>
        /// The number of bonuses that can be spent.
        /// </summary>
        [JsonIgnore]
        int AllowedWalletAmount { get; }

        /// <summary>
        /// Limit on the number of bonuses applied.
        /// </summary>
        [JsonIgnore]
        double? AvailableWalletAmount { get; }

        /// <summary>
        /// Resets discount items except wallets.
        /// </summary>
        void ResetWalletAmounts();

        /// <summary>
        /// Sets/changes the order's available amount of a wallet.
        /// </summary>
        /// <param name="amount"></param>
        void SetAvailableWalletAmount(double? amount);

        /// <summary>
        /// Sets/changes the order's allowed amount of a wallet.
        /// </summary>
        /// <param name="amount"></param>
        void SetAllowedWalletAmount(int amount);

        /// <summary>
        /// Resets the order's allowed amount of a wallet to zero.
        /// </summary>
        void ResetAllowedWalletAmount();
    }
}
