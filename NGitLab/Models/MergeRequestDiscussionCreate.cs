using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestDiscussionCreate
    {
        [DataMember(Name = "body")]
        [JsonPropertyName("body")]
        public string Body;

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt;
    }
}
