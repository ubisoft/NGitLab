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
        public bool IssuesEnabled;

        [Obsolete("Does not exist anymore in gitlab api")]
        public bool WallEnabled;

        [DataMember(Name = "merge_requests_enabled")]
        public bool MergeRequestsEnabled;

        [DataMember(Name = "snippets_enabled")]
        public bool SnippetsEnabled;

        [DataMember(Name = "wiki_enabled")]
        public bool WikiEnabled;

        [DataMember(Name = "import_url")]
        public string ImportUrl = "";

        [DataMember(Name = "visibility")]
        public VisibilityLevel VisibilityLevel;

        [DataMember(Name = "tag_list")]
        public List<string> Tags;
    }
}