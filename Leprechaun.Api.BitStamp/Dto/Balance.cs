using Newtonsoft.Json;

namespace Leprechaun.BitStamp.Api.Client
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
        public double BalanceUSD { get; set; }

        /// <summary>
        /// Get/set  BTV balance
        /// </summary>
        [JsonProperty("btc_balance")]
        public double BalanceBTC { get; set; }

        /// <summary>
        /// Get/set USD reserved in open orders
        /// </summary>
        [JsonProperty("usd_reserved")]
        public double ReservedUSD { get; set; }

        /// <summary>
        /// Get/set BTC reserved in open orders
        /// </summary>
        [JsonProperty("btc_reserved")]
        public double ReservedBTC { get; set; }

        /// <summary>
        /// Get/set USD available for trading
        /// </summary>
        [JsonProperty("usd_available")]
        public double AvailableUSD { get; set; }

        /// <summary>
        /// Get/set BTC available for trading
        /// </summary>
        [JsonProperty("btc_available")]
        public double AvailableBTC { get; set; }

        /// <summary>
        /// Get/set customer trading fee
        /// </summary>
        [JsonProperty("fee")]
        public double Fee { get; set; }

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