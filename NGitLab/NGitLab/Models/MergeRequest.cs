using System.Runtime.Serialization;

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

        [DataMember(Name="target_branch")]
        public string TargetBranch;

        [DataMember(Name="source_branch")]
        public string SourceBranch;

        [DataMember(Name="project_id")]
        public int ProjectId;

        [DataMember(Name="source_project_id")]
        public int SourceProjectId;
    }
}