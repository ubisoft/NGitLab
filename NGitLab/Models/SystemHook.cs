using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public sealed class SystemHook
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [DataMember(Name = "url")]
        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "push_events")]
        [JsonPropertyName("push_events")]
        public bool PushEvents { get; set; }

        [DataMember(Name = "tag_push_events")]
        [JsonPropertyName("tag_push_events")]
        public bool TagPushEvents { get; set; }

        [DataMember(Name = "merge_requests_events")]
        [JsonPropertyName("merge_requests_events")]
        public bool MergeRequestsEvents { get; set; }

        [DataMember(Name = "repository_update_events")]
        [JsonPropertyName("repository_update_events")]
        public bool RepositoryUpdateEvents { get; set; }

        [DataMember(Name = "enable_ssl_verification")]
        [JsonPropertyName("enable_ssl_verification")]
        public bool EnableSslVerification { get; set; }
    }
}
