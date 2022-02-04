using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public sealed class SystemHookUpsert
    {
        [Required]
        [DataMember(Name = "url")]
        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [DataMember(Name = "token")]
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [DataMember(Name = "enable_ssl_verification")]
        [JsonPropertyName("enable_ssl_verification")]
        public bool EnableSslVerification { get; set; }

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
    }
}
