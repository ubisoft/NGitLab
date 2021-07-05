using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseInfo
    {
        [DataMember(Name = "tag_name")]
        public string TagName;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "released_at")]
        public DateTime ReleasedAt;

        [DataMember(Name = "author")]
        public Author Author;

        [DataMember(Name = "commit")]
        public Commit Commit;

        [DataMember(Name = "milestones")]
        public Milestone[] Milestones;

        [DataMember(Name = "commit_path")]
        public string CommitPath;

        [DataMember(Name = "tag_path")]
        public string TagPath;

        [DataMember(Name = "assets")]
        public ReleaseAssetsInfo Assets;

        [DataMember(Name = "evidences")]
        public ReleaseEvidence[] Evidences;
    }
}
