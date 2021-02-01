using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class DiffRefs
    {
        [field: DataMember(Name = "base_sha")]
        public string BaseSha { get; set;  }

        [DataMember(Name = "head_sha")]
        public string HeadSha { get; set; }

        [DataMember(Name = "start_sha")]
        public string StartSha { get; set; }
    }
}
