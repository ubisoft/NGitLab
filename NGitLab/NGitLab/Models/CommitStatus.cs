using System;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class CommitStatus {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "status")]
        public PipelineStatus? Status { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }
        [DataMember(Name = "started_at")]
        public DateTime StartedAt { get; set; }
        [DataMember(Name = "finished_at")]
        public DateTime FinishedAt { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "allow_failure")]
        public bool AllowFailure { get; set; }
        [DataMember(Name = "author")]
        public User Author { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "sha")]
        public string Sha { get; set; }
        [DataMember(Name = "ref")]
        public string Ref { get; set; }
    }
}