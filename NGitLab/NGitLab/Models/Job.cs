using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Job
    {
        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "id")]
        public long Id;

        [DataMember(Name = "ref")]
        public string Ref;

        [DataMember(Name = "commit")]
        public Commit Commit;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "started_at")]
        public DateTime StartedAt;

        [DataMember(Name = "finished_at")]
        public DateTime FinishedAt;

        [DataMember(Name = "stage")]
        public string Stage;

        [DataMember(Name = "coverage")]
        public string Coverage;

        [DataMember(Name = "artifacts_file")]
        public JobArtifact Artifacts;

        [DataMember(Name = "runner")]
        public JobRunner Runner;

        [DataMember(Name = "status")]
        public JobStatus Status;

        [DataMember(Name = "tag")]
        public bool Tag;

        [DataMember(Name = "user")]
        public User User;

        [DataContract]
        public class JobRunner
        {
            [DataMember(Name = "id")]
            public int Id;

            [DataMember(Name = "name")]
            public string Name;

            [DataMember(Name = "active")]
            public bool Active;

            [DataMember(Name = "description")]
            public string Description;

            [DataMember(Name = "is_shared")]
            public bool IsShared;
        }

        [DataContract]
        public class JobPipeline
        {
            [DataMember(Name = "id")]
            public long Id;

            [DataMember(Name = "ref")]
            public string Ref;

            [DataMember(Name = "sha")]
            public Sha1 Sha;

            [DataMember(Name = "status")]
            public JobStatus Status;
        }

        [DataContract]
        public class JobArtifact
        {
            [DataMember(Name = "filename")]
            public string Filename;

            [DataMember(Name = "size")]
            public long Size;
        }
    }
}