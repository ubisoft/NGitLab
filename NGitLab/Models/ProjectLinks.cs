using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectLinks
    {
        [DataMember(Name = "self")]
        [JsonPropertyName("self")]
        public string Self;

        [DataMember(Name = "issues")]
        [JsonPropertyName("issues")]
        public string Issues;

        [DataMember(Name = "merge_requests")]
        [JsonPropertyName("merge_requests")]
        public string MergeRequests;

        [DataMember(Name = "repo_branches")]
        [JsonPropertyName("repo_branches")]
        public string RepoBranches;

        [DataMember(Name = "labels")]
        [JsonPropertyName("labels")]
        public string Labels;

        [DataMember(Name = "events")]
        [JsonPropertyName("events")]
        public string Events;

        [DataMember(Name = "members")]
        [JsonPropertyName("members")]
        public string Members;
    }
}
