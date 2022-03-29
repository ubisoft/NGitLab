using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Bridge : JobBasic
    {
        [DataMember(Name = "downstream_pipeline")]
        [JsonPropertyName("downstream_pipeline")]
        public JobPipeline DownstreamPipeline { get; set; }
    }
}
