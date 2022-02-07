using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Branch
    {
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "merged")]
        [JsonPropertyName("merged")]
        public bool Merged;

        [DataMember(Name = "protected")]
        [JsonPropertyName("protected")]
        public bool Protected;

        [DataMember(Name = "default")]
        [JsonPropertyName("default")]
        public bool Default;

        [DataMember(Name = "developers_can_push")]
        [JsonPropertyName("developers_can_push")]
        public bool DevelopersCanPush;

        [DataMember(Name = "developers_can_merge")]
        [JsonPropertyName("developers_can_merge")]
        public bool DevelopersCanMerge;

        [DataMember(Name = "can_push")]
        [JsonPropertyName("can_push")]
        public bool CanPush;

        [DataMember(Name = "commit")]
        [JsonPropertyName("commit")]
        public CommitInfo Commit;
    }
}
