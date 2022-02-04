using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DebuggerDisplay("{Version,nq} r{Revision,nq}")]
    [DataContract]
    public class GitLabVersion
    {
        [DataMember(Name = "version")]
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [DataMember(Name = "revision")]
        [JsonPropertyName("revision")]
        public string Revision { get; set; }
    }
}
