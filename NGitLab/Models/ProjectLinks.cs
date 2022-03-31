using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class ProjectLinks
    {
        [JsonPropertyName("self")]
        public string Self;

        [JsonPropertyName("issues")]
        public string Issues;

        [JsonPropertyName("merge_requests")]
        public string MergeRequests;

        [JsonPropertyName("repo_branches")]
        public string RepoBranches;

        [JsonPropertyName("labels")]
        public string Labels;

        [JsonPropertyName("events")]
        public string Events;

        [JsonPropertyName("members")]
        public string Members;
    }
}
