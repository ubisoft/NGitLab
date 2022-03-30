using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class PipelineBridgeQuery
    {
        [Required]
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int PipelineId { get; set; }

        public string[] Scope { get; set; }
    }
}
