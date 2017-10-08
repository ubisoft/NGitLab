using System;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class Project {
        public const string Url = "/projects";

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "default_branch")]
        public string DefaultBranch { get; set; }

        [DataMember(Name = "owner")]
        public User Owner { get; set; }

        [DataMember(Name = "public")]
        public bool Public { get; set; }

        [DataMember(Name = "path")]
        public string Path { get; set; }

        [DataMember(Name = "path_with_namespace")]
        public string PathWithNamespace { get; set; }
        [DataMember(Name = "name_with_namespace")]
        public string NameWithNamespace { get; set; }
        [DataMember(Name = "issues_enabled")]
        public bool IssuesEnabled { get; set; }

        [DataMember(Name = "merge_requests_enabled")]
        public bool MergeRequestsEnabled { get; set; }

        [DataMember(Name = "wall_enabled")]
        public bool WallEnabled { get; set; }

        [DataMember(Name = "wiki_enabled")]
        public bool WikiEnabled { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "ssh_url_to_repo")]
        public string SshUrl { get; set; }

        [DataMember(Name = "http_url_to_repo")]
        public string HttpUrl { get; set; }

        [DataMember(Name = "web_url")]
        public string WebUrl { get; set; }
        [DataMember(Name = "namespace")]
        public Namespace Namespace { get; set; }

        [DataMember(Name = "tag_list")]
        public string[] Tags { get; set; }
        [DataMember(Name = "fork?")]
        public bool Fork { get; set; }
       
        [DataMember(Name = "builds_enabled")]
        public bool BuildsEnabled { get; set; }
        [DataMember(Name = "snippets_enabled")]
        public bool SnippetsEnabled { get; set; }

        [DataMember(Name = "jobs_enabled")]
        public bool JobsEnabled { get; set; }

        [DataMember(Name = "lfs_enabled")]
        public bool LFSEnabled { get; set; }
        [DataMember(Name = "creator_id")]
        public int  CreatorID { get; set; }
 
        

    }
}