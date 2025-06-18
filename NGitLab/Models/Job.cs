using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Job : JobBasic
{
    [JsonPropertyName("artifacts_file")]
    public JobArtifact ArtifactsFile { get; set; }

    [JsonPropertyName("artifacts")]
    public JobArtifact[] Artifacts { get; set; }

    [JsonPropertyName("runner")]
    public JobRunner Runner { get; set; }

    [JsonPropertyName("runner_manager")]
    public JobRunnerManager RunnerManager { get; set; }

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

    public class JobRunnerManager
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("system_id")]
        public string SystemId { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("revision")]
        public string Revision { get; set; }

        [JsonPropertyName("platform")]
        public string Platform { get; set; }

        [JsonPropertyName("architecture")]
        public string Architecture { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("contacted_at")]
        public DateTime ContactedAt { get; set; }

        [JsonPropertyName("ip_address")]
        public string IpAddress { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
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
