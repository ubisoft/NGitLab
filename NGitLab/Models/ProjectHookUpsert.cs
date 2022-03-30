using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectHookUpsert
    {
        [Required]
        [JsonPropertyName("url")]
        public Uri Url;

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

        [JsonPropertyName("enable_ssl_verification")]
        public bool EnableSslVerification;

        [JsonPropertyName("token")]
        public string Token;
    }
}
