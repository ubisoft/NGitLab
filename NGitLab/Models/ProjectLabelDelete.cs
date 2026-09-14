using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

[Obsolete("Class used in obsolete DeleteProjectLabel method; use DeleteProjectLabelAsync instead.")]
public sealed class ProjectLabelDelete
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
