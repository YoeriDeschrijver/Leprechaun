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
        public static void Main(string[] args)
        {
            using (var client = new BitStampClient())
            {

                //Public
                //var rateInfo = client.GetRateInfo();
                //var orderBook = client.GetOrderBook();
                //var transactions = client.GetTransactions();

                //Authenticated
                //var credentials = new BitStampCredentials("t0BigagVZmWWL6mrMaMkZHkViayXvRYF", "463802", "swuqE4OiC5Jq46IkNwoUd0xiwKa2Wioo"); //Yoeri
                var credentials = new BitStampCredentials("SWG3m0JCqwpY7ieedNbRpPjnmkplv3Yr", "397277", "IzdWBoTS2uPDF0M1ZZIIl0K8u8hHPTDH"); //Kurt
                //var balance = client.GetBalance(new BitStampSignature(credentials));            
                var transactions = client.GetUserTransactions(new BitStampSignature(credentials));
                //var openOrders = client.GetOpenOrders(new BitStampSignature(credentials));
                //var order = client.Buy(new BitStampSignature(credentials), 1.01m);
            }
        }
    }
}
