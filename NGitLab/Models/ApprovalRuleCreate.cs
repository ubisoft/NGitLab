using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class ApprovalRuleCreate
    {
        /// <summary>
        /// The Id of a project.
        /// </summary>
        [Obsolete("Unnecessary in the POSTed JSON; the 'Project Id' is actually specified through the endpoint URL.")]
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Type of the rule.
        /// </summary>
        [Required]
        [JsonPropertyName("rule_type")]
        public string RuleType { get; set; }

        /// <summary>
        /// The name of the approval rule.
        /// </summary>
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The number of approvals required.
        /// </summary>
        [Required]
        [JsonPropertyName("approvals_required")]
        public int ApprovalsRequired { get; set; }

        /// <summary>
        /// The ids of users as approvers.
        /// </summary>
        [JsonPropertyName("user_ids")]
        public int[] UserIds { get; set; }

        /// <summary>
        /// The ids of groups as approvers.
        /// </summary>
        [JsonPropertyName("group_ids")]
        public int[] GroupIds { get; set; }

        /// <summary>
        /// The ids of protected branches to scope the rule by.
        /// </summary>
        [JsonPropertyName("protected_branch_ids")]
        public int[] ProtectedBranchIds { get; set; }
    }
}
