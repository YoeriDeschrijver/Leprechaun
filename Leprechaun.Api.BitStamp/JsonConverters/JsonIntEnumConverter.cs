using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Leprechaun.BitStamp.Api.Client
{
    public class JsonIntEnumConverter : JsonConverter
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
            return Enum.ToObject(objectType, reader.Value);
        }
    }
}


