using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Leprechaun.Api.BitStamp
{
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">contents of JSON object that will be deserialized</param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class JsonOrderBookConverter : JsonCreationConverter<OrderBook>
    {

        protected override OrderBook Create(Type objectType, JObject jObject)
        {
            if (FieldExists("timestamp", jObject))
            {
                //return new Artist();
            }
            //else if (FieldExists("Department", jObject))
            //{
            //    return new Employee();
            //}
            //else
            //{
            //    return new Person();
            //}
            return null;
        }

        private bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }
    }


    public class JsonBidsContverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            int a = 0;
            //writer.WriteRawValue(((DateTime)value - _epoch).TotalMilliseconds + "000");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //var test = reader.ReadAsString();
            //var obj = JObject.Load(reader);

            //var val = reader.Value;
            return null;
        }
    }
}
