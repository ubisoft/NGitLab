using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class SingleCommit : Commit
    {
        [DataMember(Name = "committed_date")]
        public DateTime CommittedDate;

        [DataMember(Name = "authored_date")]
        public DateTime AuthoredDate;

        [DataMember(Name = "parent_ids")]
        public Sha1[] Parents;
    }
}