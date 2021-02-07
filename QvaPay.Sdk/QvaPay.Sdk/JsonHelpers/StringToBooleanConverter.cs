using Newtonsoft.Json;
using System;

namespace QvaPay.Sdk.JsonHelpers
{
    public class StringToBooleanConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(bool);
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return false;
            var value = serializer.Deserialize<string>(reader);
            if (string.IsNullOrWhiteSpace(value))
                return false;
            value = value.ToLowerInvariant();
            return value == "1" || value == "true";
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var val = (bool)value ? "1" : "0";
            serializer.Serialize(writer, val);
        }

        public static readonly StringToBooleanConverter Singleton = new StringToBooleanConverter();
    }
}
