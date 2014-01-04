using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Leprechaun.Api.BitStamp
{
    /// <summary>
    /// Transaction of BitStamp user.
    /// </summary>
    public class UserTransaction
    {
        /// <summary>
        /// Get/set ID
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Get/set order ID
        /// </summary>
        [JsonProperty("order_id")]
        public string OrderID { get; set; }

        /// <summary>
        /// Get/set date (utc)
        /// </summary>
        [JsonProperty("datetime")]
        [JsonConverter(typeof(JsonTimeStampConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        /// Get/set transaction type (0 - deposit; 1 - withdrawal; 2 - market trade)
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }

        /// <summary>
        /// Get/set USD amount
        /// </summary>
        [JsonProperty("usd")]
        public decimal AmountUTC { get; set; }

        /// <summary>
        /// Get/set BTC  amount
        /// </summary>
        [JsonProperty("btc")]
        public decimal AmountBTC { get; set; }

        /// <summary>
        /// Get/set fee
        /// </summary>
        [JsonProperty("fee")]
        public decimal Fee { get; set; }
    }
}
