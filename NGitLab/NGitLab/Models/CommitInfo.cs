using System;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class CommitInfo {
        [DataMember(Name = "id")]
        public Sha1 Id { get; set; }

        [DataMember(Name = "parents")]
        public Sha1[] Parents { get; set; }

        [DataMember(Name = "tree")]
        public Sha1 Tree { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "author")]
        public PersonInfo Author { get; set; }

        [DataMember(Name = "committer")]
        public PersonInfo Committer { get; set; }

        [DataMember(Name = "authored_date")]
        public DateTime AuthoredDate { get; set; }

        [DataMember(Name = "committed_date")]
        public DateTime CommittedDate { get; set; }
    }
}