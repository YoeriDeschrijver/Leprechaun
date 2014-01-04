using System;
using Newtonsoft.Json;

namespace Leprechaun.Api.BitStamp
{
    /// <summary>
    /// Bitstamp transaction info
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Get/set ID
        /// </summary>
        [JsonProperty("tid")]
        public string ID { get; set; }

        /// <summary>
        /// Get/set date (utc)
        /// </summary>
        [JsonProperty("date")]
        [JsonConverter(typeof(JsonTimeStampConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        /// Get/set BTC price
        /// </summary>
        [JsonProperty("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Get/set BTC amount
        /// </summary>
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }
}