using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequest
    {
        public const string Url = "/merge_requests";

        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "iid")]
        public int Iid;


        [DataMember(Name = "state")]
        public string State;

        [DataMember(Name = "title")]
        public string Title;    
            
        [DataMember(Name = "assignee")]
        public User Assignee;

        [DataMember(Name = "author")]
        public User Author;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "downvotes")]
        public int Downvotes;

        [DataMember(Name = "upvotes")]
        public int Upvotes;

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt;

        [DataMember(Name="target_branch")]
        public string TargetBranch;

        [DataMember(Name="source_branch")]
        public string SourceBranch;

        [DataMember(Name="project_id")]
        public int ProjectId;

        [DataMember(Name="source_project_id")]
        public int SourceProjectId;

        [DataMember(Name = "target_project_id")]
        public int TargetProjectId;
    }
}
