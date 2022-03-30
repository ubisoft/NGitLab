using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestDiscussionCreate
    {
        [JsonPropertyName("body")]
        public string Body;

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt;
    }
}
