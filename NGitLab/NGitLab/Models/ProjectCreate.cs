using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectCreate
    {
        [Required]
        [DataMember(Name = "name")]
        public string Name;

        [Required]
        [DataMember(Name = "namespace_id")]
        public string NamespaceId;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "path")]
        public string Path;

        [DataMember(Name = "issues_enabled")]
        [Obsolete("Deprecated by Gitlab. Use IssuesAccessLevel instead")]
        public bool IssuesEnabled;

        [DataMember(Name = "issues_access_level")]
        public string IssuesAccessLevel;

        [Obsolete("Does not exist anymore in gitlab api")]
        public bool WallEnabled;

        [DataMember(Name = "merge_requests_enabled")]
        [Obsolete("Deprecated by Gitlab. Use MergeRequestsAccessLevel instead")]
        public bool MergeRequestsEnabled;

        [DataMember(Name = "merge_requests_access_level")]
        public string MergeRequestsAccessLevel;

        [DataMember(Name = "snippets_enabled")]
        [Obsolete("Deprecated by Gitlab. Use SnippetsAccessLevel instead")]
        public bool SnippetsEnabled;

        [DataMember(Name = "snippets_access_level")]
        public string SnippetsAccessLevel;

        [DataMember(Name = "wiki_enabled")]
        [Obsolete("Deprecated by Gitlab. Use WikiAccessLevel instead")]
        public bool WikiEnabled;

        [DataMember(Name = "wiki_access_level")]
        public string WikiAccessLevel;

        [DataMember(Name = "import_url")]
        public string ImportUrl = "";

        [DataMember(Name = "visibility")]
        public VisibilityLevel VisibilityLevel;

        [DataMember(Name = "tag_list")]
        public List<string> Tags;

        [DataMember(Name = "build_timeout")]
        public int? BuildTimeout;
    }
}