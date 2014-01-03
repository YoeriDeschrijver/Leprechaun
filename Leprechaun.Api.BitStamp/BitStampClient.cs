using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Leprechaun.Api.BitStamp
{
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

        public OrderBook GetOrderBook()
        {
            var response = _http.GetAsync("api/order_book/").Result;

            if (!response.IsSuccessStatusCode)
            {
                //Todo: which errors to expect?
                throw new Exception("Request went wrong...");
            }

            //var jsonSerializer = new JavaScriptSerializer();
            //var obj = jsonSerializer.Deserialize<JObject>(response.Content.ReadAsStringAsync().Result);

            var obj = JsonConvert.DeserializeObject<OrderBook>(response.Content.ReadAsStringAsync().Result /*, new JsonOrderBookConverter()*/);


            return null;
        }
        #endregion

        #region AUTHORIZATION REQUIRED
        /// <summary>
        /// Get balance.
        /// </summary>
        /// <returns></returns>
        public Balance GetBalance(BitStampCredentials credentials)
        {
            //Assemble content
            var nonce = GetNonce();
            var signature = BitStampSignature.Create(credentials, nonce);

            var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("key", credentials.ApiKey),                
                new KeyValuePair<string, string>("signature", signature),
                new KeyValuePair<string, string>("nonce", nonce.ToString())
            });

            var response = _http.PostAsync("api/balance/", content).Result;

            if (!response.IsSuccessStatusCode)
            {
                //Todo: which errors to expect?
                throw new Exception("Request went wrong...");
            }
            
            return JsonConvert.DeserializeObject<Balance>(response.Content.ReadAsStringAsync().Result);
        }
        #endregion




        /// <summary>
        /// Create nonce
        /// </summary>
        /// <returns></returns>
        private static int GetNonce() { return (int)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds; }
    }
}

