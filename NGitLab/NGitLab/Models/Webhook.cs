using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NGitLab.Models
{
    [DataContract]
    public class Webhook
    {
        public const string Url = "/hooks";

        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "url")]
        public string WebhookUrl;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "push_events")]
        public bool PushEvents;

        [DataMember(Name = "issues_events")]
        public bool IssuesEvents;

        [DataMember(Name = "tag_push_events")]
        public bool TagPushEvents;

        [DataMember(Name = "merge_requests_events")]
        public bool MergeRequestsEvents;

        [DataMember(Name = "note_events")]
        public bool NoteEvents;

        [DataMember(Name = "job_events")]
        public bool JobEvents;

        [DataMember(Name = "pipeline_events")]
        public bool PipelineEvents;

        [DataMember(Name = "wiki_page_events")]
        public bool WikiPageEvents;

        [DataMember(Name = "enable_ssl_verification")]
        public bool EnableSslVerification;
    }
}
