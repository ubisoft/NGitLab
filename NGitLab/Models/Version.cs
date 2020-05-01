using System.Diagnostics;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DebuggerDisplay("{Version,nq} r{Revision,nq}")]
    [DataContract]
    public class GitLabVersion
    {
        [DataMember(Name = "version")]
        public string Version { get; set; }

        [DataMember(Name = "revision")]
        public string Revision { get; set; }
    }
}
