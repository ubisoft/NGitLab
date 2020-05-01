using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Pipeline
    {
        public const string Url = "/pipelines";

        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "status")]
        public JobStatus Status;

        [DataMember(Name = "ref")]
        public string Ref;

        [DataMember(Name = "tag")]
        public bool Tag;

        [DataMember(Name = "sha")]
        public Sha1 Sha;

        [DataMember(Name = "before_sha")]
        public Sha1 BeforeSha;

        [DataMember(Name = "yaml_errors")]
        public string YamlError;

        [DataMember(Name = "user")]
        public User User;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt;

        [DataMember(Name = "started_at")]
        public DateTime StartedAt;

        [DataMember(Name = "finished_at")]
        public DateTime FinishedAt;

        [DataMember(Name = "committed_at")]
        public DateTime CommittedAt;

        /// <summary>
        /// Duration in seconds.
        /// </summary>
        [DataMember(Name = "duration")]
        public long? Duration;

        [DataMember(Name = "coverage")]
        public double Coverage;

        [DataMember(Name = "web_url")]
        public string WebUrl;
    }
}
