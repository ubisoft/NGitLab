using System.ComponentModel.DataAnnotations;

namespace NGitLab.Models
{
    public class PipelineBridgeQuery
    {
        [Required]
        public int PipelineId { get; set; }

        public string[] Scope { get; set; }
    }
}
