using System;
using System.Collections.Generic;
using System.Threading;
using Leprechaun.BitStamp.Api.Client;
using Leprechaun.BitStamp.DAL;

namespace Leprechaun.BitStamp.Jobs
{
    /// <summary>
    /// This job will collect the BTC rates.
    /// </summary>
    public class CollectBTCRatesJob
    {
        private bool _isStarted = false;
        private int _waitTime;

        public CollectBTCRatesJob(int waitTime = 30000)
        {
            _waitTime = waitTime;
        }

        /// <summary>
        /// Start the job
        /// </summary>
        public void Start()
        {
            _isStarted = true;

            this.Run();
        }

        /// <summary>
        /// Run the job
        /// </summary>
        private void Run()
        {
            //Put them in a temp list
            List<BTCRate> rates = new List<BTCRate>();

            using (var client = new BitStampClient())
            {
                while (_isStarted)
                {
                    //Get rate info from BitStamp
                    var rate = client.GetRateInfo();
                                        
                    //TODO: Store btc rate in database
                    rates.Add(new BTCRate
                    {
                        Date = DateTime.Now,
                        Last = rate.Last,
                        Highest = rate.Highest,
                        Lowest = rate.Lowest,
                        Volume = rate.Volume,
                        Bid = rate.Bid,
                        Ask = rate.Ask
                    });

                    //Wait before collecting next one
                    
                    Thread.Sleep(_waitTime);
                }
            }
        }

        /// <summary>
        /// Stop the job
        /// </summary>
        public void Stop()
        {
            _isStarted = false;
        }
    }
}
