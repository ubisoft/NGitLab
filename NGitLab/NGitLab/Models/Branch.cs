using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Branch
    {
        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "merged")]
        public bool Merged;

        [DataMember(Name = "protected")]
        public bool Protected;

        [DataMember(Name = "default")]
        public bool Default;

        [DataMember(Name = "developers_can_push")]
        public bool DevelopersCanPush;

        [DataMember(Name = "developers_can_merge")]
        public bool DevelopersCanMerge;

        [DataMember(Name = "can_push")]
        public bool CanPush;

        [DataMember(Name = "commit")]
        public CommitInfo Commit;
    }
}