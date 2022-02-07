using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class PipelineBasic
    {
        public const string Url = "/pipelines";

        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [DataMember(Name = "status")]
        [JsonPropertyName("status")]
        public JobStatus Status;

        [DataMember(Name = "ref")]
        [JsonPropertyName("ref")]
        public string Ref;

        [DataMember(Name = "sha")]
        [JsonPropertyName("sha")]
        public Sha1 Sha;
    }
}
