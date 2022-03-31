using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class MergeRequestDiscussionCreate
    {
        [JsonPropertyName("body")]
        public string Body;

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt;
    }
}
