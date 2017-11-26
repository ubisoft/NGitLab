using System;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class SingleCommit : Commit {
        [DataMember(Name = "committed_date")]
        public DateTime CommittedDate { get; set; }

        [DataMember(Name = "authored_date")]
        public DateTime AuthoredDate { get; set; }
    }
}