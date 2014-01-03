using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Leprechaun.Api.BitStamp
{
    public class OrderBook
    {
        /// <summary>
        /// Get/set BTC creation date
        /// </summary>
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(JsonTimeStampConverter))]
        public DateTime Created { get; set; }

        ///// <summary>
        ///// Get/set bids
        ///// </summary>
        //[JsonProperty("bids")]
        //[JsonConverter(typeof(JsonBidsContverter))]
        //public List<KeyValuePair<string, string>> Bids { get; set; }

        ///// <summary>
        ///// Get/set asks
        ///// </summary>
        //[JsonProperty("asks")]
        //public List<Ask> Asks { get; set; }

    }
}
