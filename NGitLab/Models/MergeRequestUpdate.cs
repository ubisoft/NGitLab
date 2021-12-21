using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestUpdate
    {
        [DataMember(Name = "source_branch")]
        public string SourceBranch;

        [DataMember(Name = "target_branch")]
        public string TargetBranch;

        [DataMember(Name = "assignee_id")]
        public int? AssigneeId;

        [DataMember(Name = "assignee_ids")]
        public int[] AssigneeIds;

        [DataMember(Name = "reviewer_ids")]
        public int[] ReviewerIds;

        [DataMember(Name = "title")]
        public string Title;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "state_event")]
        public string NewState;

        [DataMember(Name = "labels")]
        public string Labels;

        [DataMember(Name = "milestone_id")]
        public int? MilestoneId;

        [DataMember(Name = "allow_collaboration")]
        public bool? AllowCollaboration;
    }

    [DataContract]
    public class MergeRequestUpdateState
    {
        [DataMember(Name = "state_event")]
        public string NewState;
    }

    // ReSharper disable InconsistentNaming
    public enum MergeRequestStateEvent
    {
        close,
        reopen,
        merge,
    }
}
