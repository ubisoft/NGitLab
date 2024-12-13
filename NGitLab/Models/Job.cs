using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Job : JobBasic
{
    [JsonPropertyName("artifacts_file")]
    public JobArtifact Artifacts { get; set; }

    [JsonPropertyName("runner")]
    public JobRunner Runner { get; set; }

    [JsonPropertyName("project")]
    public JobProject Project { get; set; }

    public class JobRunner
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("is_shared")]
        public bool IsShared { get; set; }
    }

    public class JobArtifact
    {
        [JsonPropertyName("filename")]
        public string Filename { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }
    }

    public class JobProject
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("path_with_namespace")]
        public string PathWithNamespace { get; set; }
    }
}
