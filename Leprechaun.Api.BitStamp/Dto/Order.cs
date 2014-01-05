using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Leprechaun.Api.BitStamp
{
    

    public class Order
    {
        /// <summary>
        /// Get/set ID
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Get/set date (utc)
        /// </summary>
        [JsonProperty("date")]
        [JsonConverter(typeof(JsonTimeStampConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        /// Get/set type - buy or sell (0 - buy; 1 - sell)
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(JsonIntEnumConverter))]
        public OrderType Type { get; set; }

        /// <summary>
        /// Get/set BTC price
        /// </summary>
        [JsonProperty("price")]
        public double Price { get; set; }

        /// <summary>
        /// Get/set BTC amount
        /// </summary>
        [JsonProperty("amount")]
        public double Amount { get; set; }
    }
}
