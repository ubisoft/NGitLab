using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Job : JobBasic
    {
        [DataMember(Name = "artifacts_file")]
        [JsonPropertyName("artifacts_file")]
        public JobArtifact Artifacts { get; set; }

        [DataMember(Name = "runner")]
        [JsonPropertyName("runner")]
        public JobRunner Runner { get; set; }

        [DataMember(Name = "project")]
        [JsonPropertyName("project")]
        public JobProject Project { get; set; }

        [DataContract]
        public class JobRunner
        {
            [DataMember(Name = "id")]
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [DataMember(Name = "name")]
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [DataMember(Name = "active")]
            [JsonPropertyName("active")]
            public bool Active { get; set; }

            [DataMember(Name = "description")]
            [JsonPropertyName("description")]
            public string Description { get; set; }

            [DataMember(Name = "is_shared")]
            [JsonPropertyName("is_shared")]
            public bool IsShared { get; set; }
        }

        [DataContract]
        public class JobArtifact
        {
            [DataMember(Name = "filename")]
            [JsonPropertyName("filename")]
            public string Filename { get; set; }

            [DataMember(Name = "size")]
            [JsonPropertyName("size")]
            public long Size { get; set; }
        }

        [DataContract]
        public class JobProject
        {
            [DataMember(Name = "id")]
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [DataMember(Name = "name")]
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [DataMember(Name = "path_with_namespace")]
            [JsonPropertyName("path_with_namespace")]
            public string PathWithNamespace { get; set; }
        }
    }
}
