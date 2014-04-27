using Newtonsoft.Json;

namespace NGitLab.Models
{
    public class MergeRequest
    {
        public const string Url = "/merge_requests";

        public int Id;
        public int Iid;
        public string Title;
        public string State;
        public bool Closed;
        public bool Merged;
        public User Author;
        public User Assignee;

        [JsonProperty("target_branch")]
        public string TargetBranch;

        [JsonProperty("source_branch")]
        public string SourceBranch;

        [JsonProperty("project_id")]
        public int ProjectId;

        [JsonProperty("source_project_id")]
        public int SourceProjectId;
    }
}