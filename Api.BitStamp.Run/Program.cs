using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leprechaun.Api.BitStamp;

namespace Leprechaun.BitStamp.Start
{
    public class Program
    {
        //Yoeri
        //private const string ApiKey = "t0BigagVZmWWL6mrMaMkZHkViayXvRYF";
        //private const string Secret = "swuqE4OiC5Jq46IkNwoUd0xiwKa2Wioo";
        //private const string ClientID = "463802"; 

        //Kurt
        private const string ApiKey = "SWG3m0JCqwpY7ieedNbRpPjnmkplv3Yr";
        private const string Secret = "IzdWBoTS2uPDF0M1ZZIIl0K8u8hHPTDH";
        private const string ClientID = "397277";

        public static void Main(string[] args)
        {
            var client = new BitStampClient();
            var credentials = new BitStampCredentials(ApiKey, ClientID, Secret);

            //var rateInfo = client.GetRateInfo();
            //var balance = client.GetBalance(credentials);
            var orderBook = client.GetOrderBook();
            
        }
    }
}
