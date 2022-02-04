using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ApprovalRuleCreate
    {
        /// <summary>
        /// The Id of a project.
        /// </summary>
        [Required]
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Type of the rule.
        /// </summary>
        [Required]
        [DataMember(Name = "rule_type")]
        [JsonPropertyName("rule_type")]
        public string RuleType { get; set; }

        /// <summary>
        /// The name of the approval rule.
        /// </summary>
        [Required]
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The number of approvals required.
        /// </summary>
        [Required]
        [DataMember(Name = "approvals_required")]
        [JsonPropertyName("approvals_required")]
        public int ApprovalsRequired { get; set; }

        /// <summary>
        /// The ids of users as approvers.
        /// </summary>
        [DataMember(Name = "user_ids")]
        [JsonPropertyName("user_ids")]
        public int[] UserIds { get; set; }

        /// <summary>
        /// The ids of groups as approvers.
        /// </summary>
        [DataMember(Name = "group_ids")]
        [JsonPropertyName("group_ids")]
        public int[] GroupIds { get; set; }

        /// <summary>
        /// The ids of protected branches to scope the rule by.
        /// </summary>
        [DataMember(Name = "protected_branch_ids")]
        [JsonPropertyName("protected_branch_ids")]
        public int[] ProtectedBranchIds { get; set; }
    }
}
