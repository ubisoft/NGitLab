using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class CommitStats
    {
        [DataMember(Name = "additions")]
        public int Additions;

        [DataMember(Name = "deletions")]
        public int Deletions;

        [DataMember(Name = "total")]
        public int Total;
    }
}
