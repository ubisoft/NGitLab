using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class PipelineCreate
{
    [Required]
    [JsonPropertyName("ref")]
    public string Ref { get; set; }

    [JsonPropertyName("variables")]
    public IDictionary<string, string> Variables { get; } = new Dictionary<string, string>(StringComparer.Ordinal);
}
