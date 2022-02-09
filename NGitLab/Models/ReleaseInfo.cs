using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseInfo
    {
        [DataMember(Name = "tag_name")]
        [JsonPropertyName("tag_name")]
        public string TagName { get; set; }

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "released_at")]
        [JsonPropertyName("released_at")]
        public DateTime ReleasedAt { get; set; }

        [DataMember(Name = "author")]
        [JsonPropertyName("author")]
        public Author Author { get; set; }

        [DataMember(Name = "commit")]
        [JsonPropertyName("commit")]
        public Commit Commit { get; set; }

        [DataMember(Name = "milestones")]
        [JsonPropertyName("milestones")]
        public Milestone[] Milestones { get; set; }

        [DataMember(Name = "commit_path")]
        [JsonPropertyName("commit_path")]
        public string CommitPath { get; set; }

        [DataMember(Name = "tag_path")]
        [JsonPropertyName("tag_path")]
        public string TagPath { get; set; }

        [DataMember(Name = "assets")]
        [JsonPropertyName("assets")]
        public ReleaseAssetsInfo Assets { get; set; }

        [DataMember(Name = "evidences")]
        [JsonPropertyName("evidences")]
        public ReleaseEvidence[] Evidences { get; set; }
    }
}
