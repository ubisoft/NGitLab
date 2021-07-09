using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseInfo
    {
        [DataMember(Name = "tag_name")]
        public string TagName { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "released_at")]
        public DateTime ReleasedAt { get; set; }

        [DataMember(Name = "author")]
        public Author Author { get; set; }

        [DataMember(Name = "commit")]
        public Commit Commit { get; set; }

        [DataMember(Name = "milestones")]
        public Milestone[] Milestones { get; set; }

        [DataMember(Name = "commit_path")]
        public string CommitPath { get; set; }

        [DataMember(Name = "tag_path")]
        public string TagPath { get; set; }

        [DataMember(Name = "assets")]
        public ReleaseAssetsInfo Assets { get; set; }

        [DataMember(Name = "evidences")]
        public ReleaseEvidence[] Evidences { get; set; }
    }
}
