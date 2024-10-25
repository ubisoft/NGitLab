﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class PipelineSchedule
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

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

    [JsonPropertyName("last_pipeline")]
    public PipelineBasic LastPipeline { get; set; }

    [JsonPropertyName("owner")]
    public User Owner { get; set; }

    [JsonPropertyName("variables")]
    public IEnumerable<PipelineVariable> Variables { get; set; }
}
