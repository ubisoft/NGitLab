using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class MergeRequestUpdate {
        [DataMember(Name = "source_branch")]
        public string SourceBranch { get; set; }

        [DataMember(Name = "target_branch")]
        public string TargetBranch { get; set; }

        [DataMember(Name = "assignee_id")]
        public int? AssigneeId { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "state_event")]
        public string NewState { get; set; }
    }

    // ReSharper disable InconsistentNaming
    public enum MergeRequestStateEvent {
        close,
        reopen,
        merge
    }
}