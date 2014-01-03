using System;
using System.Security.Cryptography;
using System.Text;

namespace Leprechaun.Api.BitStamp
{
    public class BitStampSignature
    {
        public static string Create(BitStampCredentials credentials, int nonce)
        {
            var message = string.Format("{0}{1}{2}", nonce, credentials.ClientID, credentials.ApiKey);

            return ByteArrayToString(SignHMACSHA256(credentials.Secret, Encoding.ASCII.GetBytes(message))).ToUpper();
        }
        
        private static byte[] SignHMACSHA256(String key, byte[] data)
        {
            var hashMaker = new HMACSHA256(Encoding.ASCII.GetBytes(key));
            return hashMaker.ComputeHash(data);
        }

        private static string ByteArrayToString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
