using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    public class Project
    {
        public const string Url = "/projects";

        public int Id;
        public string Name;
        public string Description;

        [DataMember(Name = "default_branch")]
        public string DefaultBranch;

        public User Owner;
        public bool Public;
        public string Path;

        [DataMember(Name = "path_with_namespace")]
        public string PathWithNamespace;

        [DataMember(Name = "issues_enabled")]
        public bool IssuesEnabled;

        [DataMember(Name = "merge_requests_enabled")]
        public bool MergeRequestsEnabled;

        [DataMember(Name = "wall_enabled")]
        public bool WallEnabled;

        [DataMember(Name = "wiki_enabled")]
        public bool WikiEnabled;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "ssh_url_to_repo")]
        public string SshUrl;

        [DataMember(Name = "http_url_to_repo")]
        public string HttpUrl;

        public Namespace Namespace;
    }
}