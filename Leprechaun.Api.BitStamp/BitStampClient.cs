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
    public class BitStampClient
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
        /// <summary>
        /// Get balance.
        /// </summary>
        /// <returns></returns>
        public Balance GetBalance(BitStampSignature signature)
        {
            //Assemble content
            var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("key", signature.ApiKey),                
                new KeyValuePair<string, string>("signature", signature.Signature),
                new KeyValuePair<string, string>("nonce", signature.Nonce.ToString())
            });

            var response = _http.PostAsync("api/balance/", content).Result;

            if (!response.IsSuccessStatusCode)
            {
                //Todo: which errors to expect?
                throw new Exception("Request went wrong...");
            }
            
            return JsonConvert.DeserializeObject<Balance>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Get users transactions
        /// </summary>
        /// <returns></returns>
        public List<UserTransaction> GetUserTransactions(BitStampSignature signature)
        {
            //Assemble content
            var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("key", signature.ApiKey),                
                new KeyValuePair<string, string>("signature", signature.Signature),
                new KeyValuePair<string, string>("nonce", signature.Nonce.ToString())
            });

            var response = _http.PostAsync("api/user_transactions/", content).Result;

            if (!response.IsSuccessStatusCode)
            {
                //Todo: which errors to expect?
                throw new Exception("Request went wrong...");
            }
            //TODO: we also can receive an error in json on HTTP 200 ex: {"error":"Invalid nonce"} 

            return JsonConvert.DeserializeObject<List<UserTransaction>>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Get users open orders.
        /// </summary>
        /// <returns></returns>
        public List<Order> GetOpenOrders(BitStampSignature signature)
        {
            //Assemble content
            var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("key", signature.ApiKey),                
                new KeyValuePair<string, string>("signature", signature.Signature),
                new KeyValuePair<string, string>("nonce", signature.Nonce.ToString())
            });

            var response = _http.PostAsync("api/open_orders/", content).Result;

            if (!response.IsSuccessStatusCode)
            {
                //Todo: which errors to expect?
                throw new Exception("Request went wrong...");
            }
            //TODO: we also can receive an error in json on HTTP 200 ex: {"error":"Invalid nonce"} 

            return JsonConvert.DeserializeObject<List<Order>>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Cancel order
        /// </summary>
        /// <returns></returns>
        public void CancelOrder(BitStampSignature signature, string orderID)
        {
            if (string.IsNullOrWhiteSpace(orderID))
            {
                throw new ArgumentException("Invalid order ID");
            }
            
            //Assemble content
            var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("key", signature.ApiKey),                
                new KeyValuePair<string, string>("signature", signature.Signature),
                new KeyValuePair<string, string>("nonce", signature.Nonce.ToString()),
                new KeyValuePair<string, string>("id", orderID)
            });

            var response = _http.PostAsync("api/cancel_order/", content).Result;

            if (!response.IsSuccessStatusCode)
            {
                //Todo: which errors to expect?
                throw new Exception("Request went wrong...");
            }
            //TODO: we also can receive an error in json on HTTP 200 ex: {"error":"Invalid nonce"} 

            //BitStamp:
            //Returns 'true' if order has been found and canceled.

            if (!Convert.ToBoolean(JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result)))
                throw new Exception("Cancel order failed.");
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
                throw new ArgumentException("Amount is invalid");
            }

            var rate = GetRateInfo();            
            if (!price.HasValue) price = rate.Bid;

            //Assemble content
            var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("key", signature.ApiKey),                
                new KeyValuePair<string, string>("signature", signature.Signature),
                new KeyValuePair<string, string>("nonce", signature.Nonce.ToString()),
                new KeyValuePair<string, string>("amount ", amount.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("price", price.Value.ToString(CultureInfo.InvariantCulture))
            });

            var response = _http.PostAsync("api/buy/", content).Result;

            if (!response.IsSuccessStatusCode)
            {
                //Todo: which errors to expect?
                throw new Exception("Request went wrong...");
            }
            //TODO: we also can receive an error in json on HTTP 200 ex: {"error":"Invalid nonce"} 

            return JsonConvert.DeserializeObject<Order>(response.Content.ReadAsStringAsync().Result);
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

            var rate = GetRateInfo();
            if (!price.HasValue) price = rate.Ask;

            //Assemble content
            var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("key", signature.ApiKey),                
                new KeyValuePair<string, string>("signature", signature.Signature),
                new KeyValuePair<string, string>("nonce", signature.Nonce.ToString()),
                new KeyValuePair<string, string>("amount ", amount.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("price", price.Value.ToString(CultureInfo.InvariantCulture))
            });

            var response = _http.PostAsync("api/sell/", content).Result;

            if (!response.IsSuccessStatusCode)
            {
                //Todo: which errors to expect?
                throw new Exception("Request went wrong...");
            }
            //TODO: we also can receive an error in json on HTTP 200 ex: {"error":"Invalid nonce"} 

            return JsonConvert.DeserializeObject<Order>(response.Content.ReadAsStringAsync().Result);
        }
        #endregion        
    }
}

