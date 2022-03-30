using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class PipelineBridgeQuery
    {
        [Required]
        public int PipelineId { get; set; }

        public string[] Scope { get; set; }
    }
}
