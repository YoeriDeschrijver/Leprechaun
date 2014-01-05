using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Leprechaun.Api.BitStamp
{
    /// <summary>
    /// Order book is a list of all open sell and buy orders at the moment. 
    /// You can buy or sell bitcoins at those prices instantly.
    /// </summary>
    public class OrderBook
    {
        /// <summary>
        /// Get/set BTC creation date (utc)
        /// </summary>
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(JsonTimeStampConverter))]
        public DateTime Created { get; set; }

        /// <summary>
        /// Get/set bids
        /// </summary>
        [JsonProperty("bids")]
        public List<List<double>> Bids { get; set; } //I know this looks ugly but this is what we get from BitStamp { bids: [ ["721.05", "1.002"], ... ] }

        /// <summary>
        /// Get/set asks
        /// </summary>
        [JsonProperty("asks")]
        public List<List<double>> Asks { get; set; }

    }
}
