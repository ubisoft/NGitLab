using System;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class Pipeline {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "status")]
        public PipelineStatus Status { get; set; }

        [DataMember(Name = "ref")]
        public string Ref { get; set; }

        [DataMember(Name = "sha")]
        public Sha1 Sha { get; set; }

        [DataMember(Name = "before_sha")]
        public Sha1 BeforeSha { get; set; }

        [DataMember(Name = "user")]
        public User User { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime? CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [DataMember(Name = "started_at")]
        public DateTime? StartedAt { get; set; }

        [DataMember(Name = "commited_at")]
        public DateTime? CommitedAt { get; set; }
    }
}
