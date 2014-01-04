using System;
using System.Security.Cryptography;
using System.Text;

namespace Leprechaun.Api.BitStamp
{
    /// <summary>
    /// Signature is a HMAC-SHA256 encoded message containing: nonce, client ID and API key. 
    /// The HMAC-SHA256 code must be generated using a secret key that was generated with your API key. 
    /// This code must be converted to it's hexadecimal representation (64 uppercase characters).
    /// </summary>
    public class BitStampSignature
    {
        private BitStampCredentials _credentials;        
        private long _nonce;
        private string _signature;

        public BitStampSignature(BitStampCredentials credentials)
        {
            _credentials = credentials;
            _nonce = GetNonce();
            _signature = CreateSignature();
        }

        public string ApiKey { get { return _credentials.ApiKey; } }
        public string ClientID { get { return _credentials.ClientID; } }
        public long Nonce { get { return _nonce; } }
        public string Signature { get { return _signature; } }


        /// <summary>
        /// Create signature
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        private string CreateSignature()
        {
            var message = string.Format("{0}{1}{2}", _nonce, _credentials.ClientID, _credentials.ApiKey);

            return ByteArrayToString(SignHMACSHA256(_credentials.Secret, Encoding.ASCII.GetBytes(message))).ToUpper();
        }
        

        #region UTILITY
        /// <summary>
        /// Create hash
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte[] SignHMACSHA256(String key, byte[] data)
        {
            var hashMaker = new HMACSHA256(Encoding.ASCII.GetBytes(key));
            return hashMaker.ComputeHash(data);
        }

        private static string ByteArrayToString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Create nonce
        /// </summary>
        /// <returns></returns>
        private static long GetNonce() 
        { 
            return (long) (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds; 
        }
        #endregion
    }
}
