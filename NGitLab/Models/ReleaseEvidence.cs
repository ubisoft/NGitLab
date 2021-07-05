using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseEvidence
    {
        [DataMember(Name = "sha")]
        public string Sha;

        [DataMember(Name = "filepath")]
        public string Filepath;

        [DataMember(Name = "collected_at")]
        public DateTime CollectedAt;
    }
}
