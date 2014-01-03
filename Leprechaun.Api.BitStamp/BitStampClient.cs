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

namespace Leprechaun.Api.BitStamp
{
    public class BitStampClient
    {
        private const string Key = "t0BigagVZmWWL6mrMaMkZHkViayXvRYF"; //Yoeri
        private const string Secret = "swuqE4OiC5Jq46IkNwoUd0xiwKa2Wioo"; //Yoeri
        private const string ClientId = "463802"; //Yoeri

        private HttpClient _http;

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

        public Balance GetBalance()
        {
            //Assemble content
            var nonce = GetNonce();
            var signature = GetSignature(string.Format("{0}{1}{2}", nonce, ClientId, Key));

            var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("key", Key),                
                new KeyValuePair<string, string>("signature", signature),
                new KeyValuePair<string, string>("nonce", nonce.ToString(CultureInfo.InvariantCulture))
            });

            var response = _http.PostAsync("api/balance/", content).Result;
            
            //TODO: check if response has status 200
            return JsonConvert.DeserializeObject<Balance>(response.Content.ReadAsStringAsync().Result);
        }

        private static int GetNonce(){return (int)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;}
        private static string GetSignature(string message)
        {
            return ByteArrayToString(SignHMACSHA256(Secret, Encoding.ASCII.GetBytes(message))).ToUpper();
        }

        public static byte[] SignHMACSHA256(String key, byte[] data)
        {
            var hashMaker = new HMACSHA256(Encoding.ASCII.GetBytes(key));
            return hashMaker.ComputeHash(data);
        }

        public static string ByteArrayToString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}

