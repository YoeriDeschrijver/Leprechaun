using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Leprechaun.Api.BitStamp
{
    public class JsonBitStampErrorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value != null) 
                //{"error":"Invalid nonce"}
                return new List<string>(new []{ (string)reader.Value });

            try
            {
                //{"error": {"__all__": ["You need $839.9 to open that order. You have only $0.0 available. Check your account balance for details."]}}
                var errors = serializer.Deserialize<JObject>(reader);
                if (errors != null && errors["__all__"] != null)
                {
                    return errors["__all__"].ToObject<List<string>>();
                }
                return null;
            }
            catch (Exception ex) { return ""; }
        }
    }
}


