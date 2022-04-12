using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class Branch
    {
        [JsonPropertyName("name")]
        public string Name;

        [JsonPropertyName("merged")]
        public bool Merged;

        [JsonPropertyName("protected")]
        public bool Protected;

        [JsonPropertyName("default")]
        public bool Default;

        [JsonPropertyName("developers_can_push")]
        public bool DevelopersCanPush;

        [JsonPropertyName("developers_can_merge")]
        public bool DevelopersCanMerge;

        [JsonPropertyName("can_push")]
        public bool CanPush;

        [JsonPropertyName("commit")]
        public CommitInfo Commit;
    }
}
