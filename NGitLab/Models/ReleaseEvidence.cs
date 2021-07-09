using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseEvidence
    {
        [DataMember(Name = "sha")]
        public string Sha { get; set; }

        [DataMember(Name = "filepath")]
        public string Filepath { get; set; }

        [DataMember(Name = "collected_at")]
        public DateTime CollectedAt { get; set; }
    }
}
