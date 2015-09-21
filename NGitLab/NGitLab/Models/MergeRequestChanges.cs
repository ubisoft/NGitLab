using System;
using System.Runtime.Serialization;

namespace NGitLab.Models {
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

        [DataMember(Name = "work_in_progress")]
        public bool WorkInProgress;

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

        [DataMember(Name = "files")]
        public MergeRequestFileData[] Files;

        //"labels": [ ],
        //"milestone": {
        //  "id": 5,
        //  "iid": 1,
        //  "project_id": 4,
        //  "title": "v2.0",
        //  "description": "Assumenda aut placeat expedita exercitationem labore sunt enim earum.",
        //  "state": "closed",
        //  "created_at": "2015-02-02T19:49:26.013Z",
        //  "updated_at": "2015-02-02T19:49:26.013Z",
        //  "due_date": null
        //},
        //"files": [
        //  {
        //  "old_path": "VERSION",
        //  "new_path": "VERSION",
        //  "a_mode": "100644",
        //  "b_mode": "100644",
        //  "diff": "--- a/VERSION\ +++ b/VERSION\ @@ -1 +1 @@\ -1.9.7\ +1.9.8",
        //  "new_file": false,
        //  "renamed_file": false,
        //  "deleted_file": false
        //  }
        //]
    }
}
