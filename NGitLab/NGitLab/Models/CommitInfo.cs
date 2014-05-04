using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class CommitInfo
    {
        [DataMember(Name = "id")]
        public Sha1 Id;

        [DataMember(Name = "parents")]
        public Sha1[] Parents;

        [DataMember(Name = "tree")]
        public Sha1 Tree;
            
        [DataMember(Name = "message")]
        public string Message;
            
        [DataMember(Name = "author")]
        public PersonInfo Author;

        [DataMember(Name = "committer")]
        public PersonInfo Committer;

        [DataMember(Name = "authored_date")]
        public DateTime AuthoredDate;

        [DataMember(Name = "committed_date")]
        public DateTime CommittedDate;
    }
}