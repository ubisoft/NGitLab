using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    /// <summary>
    ///     Class for inserting a project hook.
    /// </summary>
    [DataContract]
    public class ProjectHookInsert {
        /// <summary>
        ///     Gets or sets the ID of the project.
        /// </summary>
        [Required]
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the hook URL.
        /// </summary>
        [Required]
        [DataMember(Name = "url")]
        public Uri Url { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to trigger hook on push events
        /// </summary>
        [DataMember(Name = "push_events")]
        public bool PushEvents { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to trigger hook on merge requests events.
        /// </summary>
        [DataMember(Name = "merge_requests_events")]
        public bool MergeRequestsEvents { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to trigger hook on issues events.
        /// </summary>
        [DataMember(Name = "issues_events")]
        public bool IssuesEvents { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to trigger hook on tag push events.
        /// </summary>
        [DataMember(Name = "tag_push_events")]
        public bool TagPushEvents { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to trigger hook on note events.
        /// </summary>
        [DataMember(Name = "note_events")]
        public bool NoteEvents { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to trigger hook on job events.
        /// </summary>
        [DataMember(Name = "job_events")]
        public bool JobEvents { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to trigger hook on pipeline events.
        /// </summary>
        [DataMember(Name = "pipeline_events")]
        public bool PipelineEvents { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to trigger hook on wiki events.
        /// </summary>
        [DataMember(Name = "wiki_events")]
        public bool WikiEvents { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to do SSL verification when triggering the hook.
        /// </summary>
        [DataMember(Name = "enable_ssl_verification")]
        public bool EnableSslVerification { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to trigger hook on confidential issues events.
        /// </summary>
        [DataMember(Name = "confidential_issues_events")]
        public bool ConfidentialIssuesEvents { get; set; }

        /// <summary>
        ///     Gets or sets a secret token to validate received payloads.
        /// </summary>
        [DataMember(Name = "token")]
        public string Token { get; set; }
    }
}