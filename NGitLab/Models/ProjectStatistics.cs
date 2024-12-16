using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ProjectStatistics
{
    [JsonPropertyName("commit_count")]
    public long CommitCount { get; set; }

    [JsonPropertyName("storage_size")]
    public long StorageSize { get; set; }

    [JsonPropertyName("repository_size")]
    public long RepositorySize { get; set; }

    [JsonPropertyName("lfs_objects_size")]
    public long LfsObjectsSize { get; set; }

    [JsonPropertyName("job_artifacts_size")]
    public long JobArtifactsSize { get; set; }
}
