using Newtonsoft.Json;
using System;

namespace QvaPay.Sdk.Models
{
    public class QvaPayTransaction
    {
        [JsonProperty("uuid")]
        public Guid Uuid { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("app_id")]
        public int AppId { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("remote_id")]
        public string RemoteId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("paid_by_user_id")]
        public int PaidByUserId { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("signed")]
        public bool Signed { get; set; }
    }
}
