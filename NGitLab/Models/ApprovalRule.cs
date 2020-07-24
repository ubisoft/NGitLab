using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public sealed class ApprovalRule
    {
        /// <summary>
        /// The rule Id.
        /// </summary>
        [DataMember(Name = "id")]
        public int RuleId { get; set; }

        /// <summary>
        /// The name of the approval rule.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "approvals_required")]
        public int ApprovalsRequired { get; set; }

        /// <summary>
        /// The users as approvers.
        /// </summary>
        [DataMember(Name = "users")]
        public User[] Users { get; set; }

        /// <summary>
        /// The groups as approvers.
        /// </summary>
        [DataMember(Name = "groups")]
        public Group[] Groups { get; set; }

        /// <summary>
        /// The protected branches to scope the rule by.
        /// </summary>
        [DataMember(Name = "protected_branches")]
        public Branch[] ProtectedBranch { get; set; }
    }
}
