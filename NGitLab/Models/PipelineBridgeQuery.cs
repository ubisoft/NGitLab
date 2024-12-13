using System.ComponentModel.DataAnnotations;

namespace NGitLab.Models;

public class PipelineBridgeQuery
{
    [Required]
    public long PipelineId { get; set; }

    public string[] Scope { get; set; }
}
