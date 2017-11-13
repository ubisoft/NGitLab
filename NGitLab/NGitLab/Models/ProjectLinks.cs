using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
