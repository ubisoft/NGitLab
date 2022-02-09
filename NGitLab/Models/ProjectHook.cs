using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectHook
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [DataMember(Name = "url")]
        [JsonPropertyName("url")]
        public Uri Url;

        [DataMember(Name = "project_id")]
        [JsonPropertyName("project_id")]
        public int ProjectId;

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "push_events")]
        [JsonPropertyName("push_events")]
        public bool PushEvents;

        [DataMember(Name = "merge_requests_events")]
        [JsonPropertyName("merge_requests_events")]
        public bool MergeRequestsEvents;

        [DataMember(Name = "issues_events")]
        [JsonPropertyName("issues_events")]
        public bool IssuesEvents;

        [DataMember(Name = "tag_push_events")]
        [JsonPropertyName("tag_push_events")]
        public bool TagPushEvents;

        [DataMember(Name = "note_events")]
        [JsonPropertyName("note_events")]
        public bool NoteEvents;

        [DataMember(Name = "job_events")]
        [JsonPropertyName("job_events")]
        public bool JobEvents;

        [DataMember(Name = "pipeline_events")]
        [JsonPropertyName("pipeline_events")]
        public bool PipelineEvents;

        [DataMember(Name = "wiki_page_events")]
        [JsonPropertyName("wiki_page_events")]
        public bool WikiPagesEvents;

        [DataMember(Name = "enable_ssl_verification")]
        [JsonPropertyName("enable_ssl_verification")]
        public bool EnableSslVerification;
    }
}
