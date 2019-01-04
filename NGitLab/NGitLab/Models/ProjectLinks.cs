using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectLinks
    {
        [DataMember(Name = "self")]
        public string Self;

        [DataMember(Name = "issues")]
        public string Issues;

        [DataMember(Name = "merge_requests")]
        public string MergeRequests;

        [DataMember(Name = "repo_branches")]
        public string RepoBranches;

        [DataMember(Name = "labels")]
        public string Labels;

        [DataMember(Name = "events")]
        public string Events;

        [DataMember(Name = "members")]
        public string Members;
    }
}
