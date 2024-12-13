using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class PipelineJobQuery
{
    [Required]
    [JsonPropertyName("id")]
    public long PipelineId { get; set; }

    public string[] Scope { get; set; }

    public bool? IncludeRetried { get; set; }
}
