using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leprechaun.Api.BitStamp
{
    public class Ask
    {
        /// <summary>
        /// Amount in USD
        /// </summary>
        public decimal USD { get; set; }

        /// <summary>
        /// Amount in BTC
        /// </summary>
        public decimal BTC { get; set; }
        
    }
}
