using Newtonsoft.Json;
using System;

namespace QvaPay.Sdk.Models
{
    public class QvaPayInvoiceInfo
    {
        [JsonProperty("app_id")]
        public Guid AppId { get; set; }

        [JsonProperty("app_secret")]
        public string AppSecret { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("remote_id")]
        public string RemoteId { get; set; }

        [JsonProperty("signed")]
        public bool Signed { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("signedUrl")]
        public string SignedUrl { get; set; }
    }
}
