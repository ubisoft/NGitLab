using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectHookUpsert
    {
        [Required]
        [DataMember(Name = "url")]
        public Uri Url;

        [DataMember(Name = "push_events")]
        public bool PushEvents;

        [DataMember(Name = "merge_requests_events")]
        public bool MergeRequestsEvents;

        [DataMember(Name = "issues_events")]
        public bool IssuesEvents;

        [DataMember(Name = "tag_push_events")]
        public bool TagPushEvents;

        [DataMember(Name = "note_events")]
        public bool NoteEvents;

        [DataMember(Name = "job_events")]
        public bool JobEvents;

        [DataMember(Name = "pipeline_events")]
        public bool PipelineEvents;
        
        [DataMember(Name = "enable_ssl_verification")]
        public bool EnableSslVerification;

        [DataMember(Name = "token")]
        public string Token;
    }
}