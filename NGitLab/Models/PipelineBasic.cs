using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class PipelineBasic
    {
        public const string Url = "/pipelines";

        [JsonPropertyName("id")]
        public int Id;

        [JsonPropertyName("status")]
        public JobStatus Status;

        [JsonPropertyName("ref")]
        public string Ref;

        [JsonPropertyName("sha")]
        public Sha1 Sha;
    }
}
