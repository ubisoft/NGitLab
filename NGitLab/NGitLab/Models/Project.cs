using System;
using Newtonsoft.Json;

namespace NGitLab.Models
{
    public class Project
    {
        public const string Url = "/projects";

        public int Id;
        public string Name;
        public string Description;

        [JsonProperty("default_branch")]
        public string DefaultBranch;

        public User Owner;
        public bool Public;
        public string Path;

        [JsonProperty("path_with_namespace")]
        public string PathWithNamespace;

        [JsonProperty("issues_enabled")]
        public bool IssuesEnabled;

        [JsonProperty("merge_requests_enabled")]
        public bool MergeRequestsEnabled;

        [JsonProperty("wall_enabled")]
        public bool WallEnabled;

        [JsonProperty("wiki_enabled")]
        public bool WikiEnabled;

        [JsonProperty("created_at")]
        public DateTime CreatedAt;

        [JsonProperty("ssh_url_to_repo")]
        public string SshUrl;

        [JsonProperty("http_url_to_repo")]
        public string HttpUrl;

        public Namespace Namespace;
    }
}