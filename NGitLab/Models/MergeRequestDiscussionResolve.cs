using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestDiscussionResolve
    {
        [DataMember(Name = "discussion_id")]
        [JsonPropertyName("discussion_id")]
        public string Id;

        [DataMember(Name = "resolved")]
        [JsonPropertyName("resolved")]
        public bool Resolved;
    }
}
