using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestCommentCreate
    {
        [JsonPropertyName("body")]
        public string Body;

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt;
    }
}
