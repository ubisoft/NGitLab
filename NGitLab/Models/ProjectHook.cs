using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectHook
    {
        [JsonPropertyName("id")]
        public int Id;

        [JsonPropertyName("url")]
        public Uri Url;

        [JsonPropertyName("project_id")]
        public int ProjectId;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [JsonPropertyName("push_events")]
        public bool PushEvents;

        [JsonPropertyName("merge_requests_events")]
        public bool MergeRequestsEvents;

        [JsonPropertyName("issues_events")]
        public bool IssuesEvents;

        [JsonPropertyName("tag_push_events")]
        public bool TagPushEvents;

        [JsonPropertyName("note_events")]
        public bool NoteEvents;

        [JsonPropertyName("job_events")]
        public bool JobEvents;

        [JsonPropertyName("pipeline_events")]
        public bool PipelineEvents;

        [JsonPropertyName("wiki_page_events")]
        public bool WikiPagesEvents;

        [JsonPropertyName("enable_ssl_verification")]
        public bool EnableSslVerification;
    }
}
