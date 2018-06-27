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


        public override bool Equals(Object obj)
        {
            if (obj != null && obj.GetType() == typeof(Webhook))
            {
                Webhook webhook = (Webhook)obj;
                return WebhookUrl == webhook.WebhookUrl &&
                       PushEvents == webhook.PushEvents &&
                       IssuesEvents == webhook.IssuesEvents &&
                       TagPushEvents == webhook.TagPushEvents &&
                       MergeRequestsEvents == webhook.MergeRequestsEvents &&
                       NoteEvents == webhook.NoteEvents &&
                       JobEvents == webhook.JobEvents &&
                       PipelineEvents == webhook.PipelineEvents &&
                       WikiPageEvents == webhook.WikiPageEvents &&
                       EnableSslVerification == webhook.EnableSslVerification;
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 727435646;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(WebhookUrl);
            hashCode = hashCode * -1521134295 + PushEvents.GetHashCode();
            hashCode = hashCode * -1521134295 + IssuesEvents.GetHashCode();
            hashCode = hashCode * -1521134295 + TagPushEvents.GetHashCode();
            hashCode = hashCode * -1521134295 + MergeRequestsEvents.GetHashCode();
            hashCode = hashCode * -1521134295 + NoteEvents.GetHashCode();
            hashCode = hashCode * -1521134295 + JobEvents.GetHashCode();
            hashCode = hashCode * -1521134295 + PipelineEvents.GetHashCode();
            hashCode = hashCode * -1521134295 + WikiPageEvents.GetHashCode();
            hashCode = hashCode * -1521134295 + EnableSslVerification.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"WebhookUrl : {WebhookUrl}\n" +
                   $"PushEvents : {PushEvents}\n" +
                   $"IssuesEvents : {IssuesEvents}\n" +
                   $"TagPushEvents : {TagPushEvents}\n" +
                   $"MergeRequestsEvents : {MergeRequestsEvents}\n" +
                   $"NoteEvents : {NoteEvents}\n" +
                   $"JobEvents : {JobEvents}\n" +
                   $"PipelineEvents : {PipelineEvents}\n" +
                   $"WikiPageEvents : {WikiPageEvents}\n" +
                   $"EnableSslVerification : {EnableSslVerification}\n";
        }
    }
}
