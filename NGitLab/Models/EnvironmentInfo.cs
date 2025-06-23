﻿using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class EnvironmentInfo
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("external_url")]
    public string ExternalUrl { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("tier")]
    public string Tier { get; set; }

    [JsonPropertyName("last_deployment")]
    public EnvironmentLastDeployment LastDeployment { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("auto_stop_at")]
    public DateTime? AutoStopAt { get; set; }
}
