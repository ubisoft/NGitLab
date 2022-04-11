using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class ProjectStatistics
    {
        [JsonPropertyName("commit_count")]
        public long CommitCount;

        [JsonPropertyName("storage_size")]
        public long StorageSize;

        [JsonPropertyName("repository_size")]
        public long RepositorySize;

        [JsonPropertyName("lfs_objects_size")]
        public long LfsObjectsSize;

        [JsonPropertyName("job_artifacts_size")]
        public long JobArtifactsSize;
    }
}
