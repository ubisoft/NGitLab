using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestApprovals
    {
        [DataMember(Name = "approvers")]
        public MergeRequestApprover[] Approvers;
    }

    [DataContract]
    public class MergeRequestApproversChange
    {
        private static readonly int[] EmptyIntArray = new int[0];

        [DataMember(Name = "approver_ids")]
        public int[] Approvers = EmptyIntArray;

        [DataMember(Name = "approver_group_ids")]
        public int[] ApproverGroups = EmptyIntArray;
    }
}
