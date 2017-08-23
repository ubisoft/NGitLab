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
        public bool IssuesEvents { get; set; }

        [DataMember(Name = "tag_push_events")]
        public bool TagPushEvents { get; set; }

        [DataMember(Name = "note_events")]
        public bool NoteEvents { get; set; }

        [DataMember(Name = "job_events")]
        public bool JobEvents { get; set; }

        [DataMember(Name = "pipeline_events")]
        public bool PipelineEvents { get; set; }

        [DataMember(Name = "wiki_page_events")]
        public bool WikiPageEvents { get; set; }

        [DataMember(Name = "enable_ssl_verification")]
        public bool EnableSslVerification { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }
    }
}