using System;
using System.Runtime.Serialization;

namespace NGitLab.Models {

    [DataContract]
    public class Job {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "pipeline")]
        public PipelineData Pipeline { get; set; }

        [DataMember(Name = "commit")]
        public Commit Commit { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime? CreatedAt { get; set; }

        [DataMember(Name = "finished_at")]
        public DateTime? FinishedAt { get; set; }

        [DataMember(Name = "artifacts_file")]
        public ArtifactsFile File { get; set; }
    }
}
