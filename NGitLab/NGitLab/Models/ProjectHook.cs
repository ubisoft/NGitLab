using System;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class ProjectHook {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "url")]
        public Uri Url { get; set; }

        [DataMember(Name = "project_id")]
        public int ProjectId { get; set; }

        [DataMember(Name = "push_events")]
        public bool PushEvents { get; set; }

        [DataMember(Name = "merge_requests_events")]
        public bool MergeRequestsEvents { get; set; }

        [DataMember(Name = "issues_events")]
        public bool issues_events { get; set; }

        [DataMember(Name = "tag_push_events")]
        public bool tag_push_events { get; set; }

        [DataMember(Name = "note_events")]
        public bool note_events { get; set; }

        [DataMember(Name = "job_events")]
        public bool job_events { get; set; }

        [DataMember(Name = "pipeline_events")]
        public bool pipeline_events { get; set; }

        [DataMember(Name = "wiki_page_events")]
        public bool wiki_page_events { get; set; }

        [DataMember(Name = "enable_ssl_verification")]
        public bool enable_ssl_verification { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }
    }
}