using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Leprechaun.Api.BitStamp
{
    /// <summary>
    /// Info: Do not make more than 600 request per 10 minutes or BitStamp will ban your IP address.
    /// </summary>
    public class BitStampClient : IDisposable
    {
        private HttpClient _http;

        /// <summary>
        /// Create new BitStampClient.
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="path"></param>
        public BitStampClient(string scheme = "https", string host = "www.bitstamp.net", int port = 443, string path = "api")
        {
            _http = new HttpClient(new HttpClientHandler())
            {
                BaseAddress = (new UriBuilder(scheme, host, port, path)).Uri
            };

            //Add headers
            _http.DefaultRequestHeaders.Add("User-Agent", string.Format("{0} {1}", typeof(BitStampClient).Assembly.GetName().Name, typeof(BitStampClient).Assembly.GetName().Version.ToString(4)));
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        #region NO AUTHORIZATION
        /// <summary>
        /// Get the lastest rate info.
        /// </summary>
        /// <returns></returns>
        public RateInfo GetRateInfo()
        {
            var response = _http.GetAsync("api/ticker/").Result;

            if (!response.IsSuccessStatusCode)
            {
                //Todo: which errors to expect?
                throw new Exception("Request went wrong...");
            }

            return JsonConvert.DeserializeObject<RateInfo>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Get the order book
        /// </summary>
        /// <returns></returns>
        public OrderBook GetOrderBook()
        {
            var response = _http.GetAsync("api/order_book/").Result;

            if (!response.IsSuccessStatusCode)
            {
                //Todo: which errors to expect?
                throw new Exception("Request went wrong...");
            }

            return JsonConvert.DeserializeObject<OrderBook>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Get the transactions
        /// </summary>
        /// <returns></returns>
        public List<Transaction> GetTransactions()
        {
            var response = _http.GetAsync("api/transactions/").Result;

            if (!response.IsSuccessStatusCode)
            {
                //Todo: which errors to expect?
                throw new Exception("Request went wrong...");
            }

            return JsonConvert.DeserializeObject<List<Transaction>>(response.Content.ReadAsStringAsync().Result);
        }        
        #endregion


        #region AUTHORIZATION REQUIRED

        public string GetBitCoinDepositAddress(BitStampSignature signature)
        {
            return PostAuthenticatedRequest<string>("api/bitcoin_deposit_address/", signature);
        }

        /// <summary>
        /// Get balance.
        /// </summary>
        /// <returns></returns>
        public Balance GetBalance(BitStampSignature signature)
        {
            return PostAuthenticatedRequest<Balance>("api/balance/", signature);
        }

        /// <summary>
        /// Get users transactions
        /// </summary>
        /// <returns></returns>
        public List<UserTransaction> GetUserTransactions(BitStampSignature signature, int offset = 0, int limit = 100, string sort = "desc")
        {
            var param = new[] {
                new KeyValuePair<string, string>("offset", offset.ToString()),
                new KeyValuePair<string, string>("limit", limit.ToString()),
                new KeyValuePair<string, string>("sort", sort)
            };
            return PostAuthenticatedRequest<List<UserTransaction>>("api/user_transactions/", signature, param);
        }

        /// <summary>
        /// Get users open orders.
        /// </summary>
        /// <returns></returns>
        public List<Order> GetOpenOrders(BitStampSignature signature)
        {
            return PostAuthenticatedRequest<List<Order>>("api/open_orders/", signature);
        }

        /// <summary>
        /// Cancel order
        /// </summary>
        /// <returns></returns>
        public bool CancelOrder(BitStampSignature signature, string orderID)
        {
            if (string.IsNullOrWhiteSpace(orderID))
            {
                throw new ArgumentException("Invalid order ID");
            }
            
            var param = new[] 
            {
                new KeyValuePair<string, string>("id", orderID)
            };

            return PostAuthenticatedRequest<bool>("api/cancel_order/", signature, param);
        }

        /// <summary>
        /// Buy Bitcoins. This uses limited orders.
        /// </summary>
        /// <param name="signature">Signature</param>
        /// <param name="amount">Amount</param>
        /// <param name="price">Price in USD</param>
        /// <returns>Order</returns>
        public Order Buy(BitStampSignature signature, decimal amount, decimal? price = null)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Invalid amount");
            }

            if (price.HasValue && price <= 0)
            {
                throw new ArgumentException("Invalid price");
            }

            //Check actual bid rate when price is empty
            var rate = GetRateInfo();            
            if (!price.HasValue) price = rate.Bid;

            var param = new[] 
            {
                new KeyValuePair<string, string>("amount", amount.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("price", price.Value.ToString(CultureInfo.InvariantCulture))
            };

            return PostAuthenticatedRequest<Order>("api/buy/", signature, param);
        }

        /// <summary>
        /// Sell Bitcoins. This uses limited orders.
        /// </summary>
        /// <param name="signature">Signature</param>
        /// <param name="amount">Amount</param>
        /// <param name="price">Price in USD</param>
        /// <returns>Order</returns>
        public Order Sell(BitStampSignature signature, decimal amount, decimal? price = null)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount is invalid");
            }

            if (price.HasValue && price <= 0)
            {
                throw new ArgumentException("Invalid price");
            }

            //Check actual ask rate when price is empty
            var rate = GetRateInfo();
            if (!price.HasValue) price = rate.Ask;

            var param = new[] 
            {
                new KeyValuePair<string, string>("amount", amount.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("price", price.Value.ToString(CultureInfo.InvariantCulture))
            };

            return PostAuthenticatedRequest<Order>("api/sell/", signature, param);
        }
        #endregion   
     

        #region HELPERS
        /// <summary>
        /// Post an authenticated request to BitStamp
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="signature"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private T PostAuthenticatedRequest<T>(string path, BitStampSignature signature, IEnumerable<KeyValuePair<string, string>> param = null)
        {
            if (signature == null)
            {
                throw new ArgumentException("Invalid signature");
            }

            var allParam = new List<KeyValuePair<string, string>>(new[] 
            {
                new KeyValuePair<string, string>("key", signature.ApiKey),           
                new KeyValuePair<string, string>("signature", signature.Signature),
                new KeyValuePair<string, string>("nonce", signature.Nonce.ToString())
            });

            if (param != null)
            {
                allParam.AddRange(param);
            }

            return PostRequest<T>(path, allParam);
        }

        /// <summary>
        /// Post a request to BitStamp
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private T PostRequest<T>(string path, IEnumerable<KeyValuePair<string, string>> param = null)
        {
            //Assemble content
            var content = new FormUrlEncodedContent(param);

            var response = _http.PostAsync(path, new FormUrlEncodedContent(param)).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new BitStampException(string.Format("Invalid request: POST HTTP {0}: {1}.", response.StatusCode , response.RequestMessage.RequestUri));
            }

            var responseContent = response.Content.ReadAsStringAsync().Result;

            Error error = null;
            try
            {
                //we also can receive an error in json on HTTP 200
                //We need to check for error first.
                //Of course this can crash if the object isn't bitstamps error, which is good :-)
                error = JsonConvert.DeserializeObject<Error>(responseContent);
            }
            catch (Exception ex) { /*It should be normal to crash here...*/ }

            if(error != null && error.Messages != null && error.Messages.Count > 0)
            {
                throw new BitStampException(error);
            }

            //Deserialize successful response
            return JsonConvert.DeserializeObject<T>(responseContent);
        }
        #endregion


        #region IDisposable Members
        public void Dispose()
        {
            //Dispose the HttpClient
            if (_http != null)
            {
                _http.Dispose();
            }
        }
        #endregion
    }
}

