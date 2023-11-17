using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public sealed class JobArtifactQuery
    {
        [JsonPropertyName("ref_name")]
        public string RefName;

        [JsonPropertyName("artifact_path")]
        public string ArtifactPath;

        [JsonPropertyName("job")]
        public string JobName;
    }
}
