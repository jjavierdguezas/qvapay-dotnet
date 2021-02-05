using Newtonsoft.Json;
using System;

namespace QvaPay.Sdk.Models
{
    public class QvaPayAppInfo
    {
        [JsonProperty("user_id")]
        public Guid UserId { get; set; }

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
        public bool Active { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
}
