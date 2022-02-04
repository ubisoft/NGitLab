using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using NGitLab.Extensions;

namespace NGitLab.Models
{
    /// <summary>
    /// Events are user activity such as commenting a merge request.
    /// </summary>
    [DataContract]
    public class Event
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [DataMember(Name = "title")]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [DataMember(Name = "project_id")]
        [JsonPropertyName("project_id")]
        public int ProjectId { get; set; }

        [DataMember(Name = "action_name")]
        [JsonPropertyName("action_name")]
        public DynamicEnum<EventAction> Action { get; set; }

        [DataMember(Name = "target_id")]
        [JsonPropertyName("target_id")]
        public long? TargetId { get; set; }

        [DataMember(Name = "target_iid")]
        [JsonPropertyName("target_iid")]
        public long? TargetIId { get; set; }

        [DataMember(Name = "target_type")]
        [JsonPropertyName("target_type")]
        public DynamicEnum<EventTargetType> TargetType { get; set; }

        [DataMember(Name = "target_title")]
        [JsonPropertyName("target_title")]
        public string TargetTitle { get; set; }

        [DataMember(Name = "author_id")]
        [JsonPropertyName("author_id")]
        public int AuthorId { get; set; }

        [DataMember(Name = "author_username")]
        [JsonPropertyName("author_username")]
        public string AuthorUserName { get; set; }

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "note")]
        [JsonPropertyName("note")]
        public Note Note { get; set; }

        [DataMember(Name = "push_data")]
        [JsonPropertyName("push_data")]
        public PushData PushData { get; set; }

        /// <summary>
        /// The target is either a GitLab object (like a merge request)
        /// or a commit object
        /// </summary>
        public string ResolvedTargetTitle
        {
            get
            {
                if (TargetTitle != null)
                {
                    return $"{TargetType} '{TargetTitle}'";
                }

                if (PushData != null)
                {
                    return $"{PushData.RefType} '{PushData.Ref}'";
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Debug display
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(ProjectId.ToString());
        }

        public string ToString(string projectName)
        {
            return $"{AuthorUserName} {Action} {ResolvedTargetTitle} at {projectName} ({GetAge(CreatedAt)})";
        }

        private static string GetAge(DateTime date)
        {
            var age = DateTime.UtcNow.Subtract(date);

            if (age.TotalDays > 1)
                return age.TotalDays.ToStringInvariant("0") + " days ago";

            if (age.TotalHours > 1)
                return age.Hours.ToStringInvariant("0") + " hours ago";

            return age.Minutes.ToStringInvariant("0") + " minutes ago";
        }
    }
}
