using Newtonsoft.Json;

namespace Leprechaun.Api.BitStamp
{
    /// <summary>
    /// Get balance info for user
    /// </summary>
    public class Balance
    {
        /// <summary>
        /// Get/set USD balance
        /// </summary>
        [JsonProperty("usd_balance")]
        public decimal BalanceUSD { get; set; }

        /// <summary>
        /// Get/set  BTV balance
        /// </summary>
        [JsonProperty("btc_balance")]
        public decimal BalanceBTC { get; set; }

        /// <summary>
        /// Get/set USD reserved in open orders
        /// </summary>
        [JsonProperty("usd_reserved")]
        public decimal ReservedUSD { get; set; }

        /// <summary>
        /// Get/set BTC reserved in open orders
        /// </summary>
        [JsonProperty("btc_reserved")]
        public decimal ReservedBTC { get; set; }

        /// <summary>
        /// Get/set USD available for trading
        /// </summary>
        [JsonProperty("usd_available")]
        public decimal AvailableUSD { get; set; }

        /// <summary>
        /// Get/set BTC available for trading
        /// </summary>
        [JsonProperty("btc_available")]
        public decimal AvailableBTC { get; set; }

        /// <summary>
        /// Get/set customer trading fee
        /// </summary>
        [JsonProperty("fee")]
        public decimal Fee { get;set;}

        /// <summary>
        /// Return object as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Balance{usdBalance={0}, btcBalance={1}, usdReserved={2}, btcReserved={3}, usdAvailable={4}, btcAvailable={5}, fee={6}}",
                BalanceUSD, BalanceBTC, ReservedUSD, ReservedBTC, AvailableUSD, AvailableBTC, Fee);
        }
    }
}