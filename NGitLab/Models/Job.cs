using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Job : JobCommon
    {
        [DataMember(Name = "pipeline")]
        [JsonPropertyName("pipeline")]
        public JobPipeline Pipeline;

        [DataMember(Name = "artifacts_file")]
        [JsonPropertyName("artifacts_file")]
        public JobArtifact Artifacts;

        [DataMember(Name = "runner")]
        [JsonPropertyName("runner")]
        public JobRunner Runner;

        [DataMember(Name = "project")]
        [JsonPropertyName("project")]
        public JobProject Project;

        [DataContract]
        public class JobRunner
        {
            [DataMember(Name = "id")]
            [JsonPropertyName("id")]
            public int Id;

            [DataMember(Name = "name")]
            [JsonPropertyName("name")]
            public string Name;

            [DataMember(Name = "active")]
            [JsonPropertyName("active")]
            public bool Active;

            [DataMember(Name = "description")]
            [JsonPropertyName("description")]
            public string Description;

            [DataMember(Name = "is_shared")]
            [JsonPropertyName("is_shared")]
            public bool IsShared;
        }

        [DataContract]
        public class JobArtifact
        {
            [DataMember(Name = "filename")]
            [JsonPropertyName("filename")]
            public string Filename;

            [DataMember(Name = "size")]
            [JsonPropertyName("size")]
            public long Size;
        }

        [DataContract]
        public class JobProject
        {
            [DataMember(Name = "id")]
            [JsonPropertyName("id")]
            public int Id;

            [DataMember(Name = "name")]
            [JsonPropertyName("name")]
            public string Name;

            [DataMember(Name = "path_with_namespace")]
            [JsonPropertyName("path_with_namespace")]
            public string PathWithNamespace;
        }
    }
}
