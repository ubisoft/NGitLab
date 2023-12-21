using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Bridge : JobBasic
{
    [JsonPropertyName("downstream_pipeline")]
    public JobPipeline DownstreamPipeline { get; set; }
}
