using System.Linq;
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

        [DataMember(Name = "title")]
        public string Title;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "labels")]
        public string Labels;

        [DataMember(Name = "milestone_id")]
        public int MilestoneId;

        public static MergeRequestUpdate FromMergeRequest(MergeRequest mergeRequest)
        {
            return new MergeRequestUpdate
            {
                SourceBranch = mergeRequest.SourceBranch,
                TargetBranch = mergeRequest.TargetBranch,
                AssigneeId = mergeRequest.Assignee?.Id ?? 0,
                Title = mergeRequest.Title,
                Description = mergeRequest.Description,
                Labels = string.Join(",", mergeRequest.Labels),
                MilestoneId = mergeRequest.Milestone?.Id ?? 0
            };
        }
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
        merge
    }
}