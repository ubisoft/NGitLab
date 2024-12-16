using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class EnvironmentLastDeployment
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("iid")]
    public long IId { get; set; }

    [JsonPropertyName("ref")]
    public string Ref { get; set; }

    [JsonPropertyName("sha")]
    public Sha1 Sha { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("user")]
    public User User { get; set; }

    [JsonPropertyName("deployable")]
    public Job Deployable { get; set; }

    [JsonPropertyName("status")]
    public JobStatus Status { get; set; }
}
