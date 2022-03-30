using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestDiscussionResolve
    {
        [JsonPropertyName("discussion_id")]
        public string Id;

        [JsonPropertyName("resolved")]
        public bool Resolved;
    }
}
