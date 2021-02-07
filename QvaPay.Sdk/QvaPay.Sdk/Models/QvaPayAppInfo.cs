using Newtonsoft.Json;
using QvaPay.Sdk.JsonHelpers;
using System;

namespace QvaPay.Sdk.Models
{
    public class QvaPayAppInfo
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("callback")]
        public string Callback { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("uuid")]
        public Guid Uuid { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("active")]
        [JsonConverter(typeof(StringToBooleanConverter))]
        public bool Active { get; set; }

        [JsonProperty("enabled")]
        [JsonConverter(typeof(StringToBooleanConverter))]
        public bool Enabled { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
