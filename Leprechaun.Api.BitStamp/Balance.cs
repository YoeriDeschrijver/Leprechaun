using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}