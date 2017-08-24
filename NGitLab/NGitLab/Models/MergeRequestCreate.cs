using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class MergeRequestCreate {
        [DataMember(Name = "source_branch")]
        public string SourceBranch { get; set; }

        [DataMember(Name = "target_branch")]
        public string TargetBranch { get; set; }

        [DataMember(Name = "assignee_id")]
        public int? AssigneeId { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }
        
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "target_project_id")]
        public int? TargetProjectId { get; set; }
    }
}