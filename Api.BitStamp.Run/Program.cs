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
            var client = new BitStampClient();
            var balance = client.GetBalance();

        }
    }
}
