using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Leprechaun.BitStamp.Api.Client
{
    /// <summary>
    /// Info about the rate of BTC in USD.
    /// </summary>
    public class RateInfo
    {
        /// <summary>
        /// Get/set last BTC price
        /// </summary>
        [JsonProperty("last")]
        public double Last { get; set; }

        /// <summary>
        /// Get/set last 24 hours price high
        /// </summary>
        [JsonProperty("high")]
        public double Highest { get; set; }

        /// <summary>
        /// Get/set last 24 hours price low
        /// </summary>
        [JsonProperty("low")]
        public double Lowest { get; set; }

        /// <summary>
        /// Get/set last 24 hours volume
        /// </summary>
        [JsonProperty("volume")]
        public double Volume { get; set; }

        /// <summary>
        /// Get/set highest buy order
        /// </summary>
        [JsonProperty("bid")]
        public double Bid { get; set; }

        /// <summary>
        /// Get/set lowest sell order
        /// </summary>
        [JsonProperty("ask")]
        public double Ask { get; set; }
    }
}
