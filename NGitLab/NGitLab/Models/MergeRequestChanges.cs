using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class MergeRequestChanges {
        public const string Url = "/changes";

        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "iid")]
        public int Iid;

        [DataMember(Name = "project_id")]
        public int ProjectId;

        [DataMember(Name = "title")]
        public string Title;

        [DataMember(Name = "description")]
        public string Description;
        //null or 0 since 8.6.1
        [DataMember(Name = "work_in_progress")]
        public bool? WorkInProgress;

        [DataMember(Name = "state")]
        public string State;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt;

        [DataMember(Name = "target_branch")]
        public string TargetBranch;

        [DataMember(Name = "source_branch")]
        public string SourceBranch;

        [DataMember(Name = "upvotes")]
        public int Upvotes;

        [DataMember(Name = "downvotes")]
        public int Downvotes;

        [DataMember(Name = "author")]
        public User Author;

        [DataMember(Name = "assignee")]
        public User Assignee;

        [DataMember(Name = "source_project_id")]
        public int SourceProjectId;

        [DataMember(Name = "target_project_id")]
        public int TargetProjectId;

        [DataMember(Name = "changes")]
        public MergeRequestFileData[] Files;
    }
}
