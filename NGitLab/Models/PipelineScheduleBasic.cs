using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class PipelineScheduleBasic
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("ref")]
    public string Ref { get; set; }

    [JsonPropertyName("cron")]
    public string Cron { get; set; }

    [JsonPropertyName("next_run_at")]
    public DateTime NextRunAt { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("owner")]
    public User Owner { get; set; }
}
