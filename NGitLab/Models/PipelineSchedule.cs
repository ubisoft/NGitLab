using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class PipelineSchedule : PipelineScheduleBasic
{
    [JsonPropertyName("last_pipeline")]
    public PipelineBasic LastPipeline { get; set; }

    [JsonPropertyName("variables")]
    public IEnumerable<PipelineVariable> Variables { get; set; }
}
