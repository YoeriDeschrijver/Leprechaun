namespace Leprechaun.Api.BitStamp
{
    /// <summary>
    /// Holds credentials for a BitStamp account.
    /// </summary>
    public class BitStampCredentials
    {
        public BitStampCredentials(string apiKey, string clientID, string secret)
        {
            ApiKey = apiKey;
            ClientID = clientID;
            Secret = secret;
        }

        /// <summary>
        /// Get/set API key
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Get/set client ID
        /// </summary>
        public string ClientID { get; set; }

        /// <summary>
        /// Get/set secret
        /// </summary>
        public string Secret { get; set; }
    }
}
