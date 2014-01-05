using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Leprechaun.BitStamp.Api.Client
{
    /// <summary>
    /// BitStamp sends error messages in different syntaxes. -> Painfull
    /// ex 1: {"error":"Invalid nonce"}
    /// ex 2: {"error": {"__all__": ["You need $839.9 to open that order. You have only $0.0 available. Check your account balance for details."]}}
    /// </summary>
    public class Error
    {
        [JsonProperty("error")]
        [JsonConverter(typeof(JsonBitStampErrorConverter))]
        public List<string> Messages { get; set; }
    }
}
