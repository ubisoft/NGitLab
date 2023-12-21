using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed class JobArtifactQuery
{
    [JsonPropertyName("ref_name")]
    public string RefName { get; set; }

    [JsonPropertyName("artifact_path")]
    public string ArtifactPath { get; set; }

    [JsonPropertyName("job")]
    public string JobName { get; set; }
}
