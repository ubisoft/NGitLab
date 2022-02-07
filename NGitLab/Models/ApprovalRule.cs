using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public sealed class ApprovalRule
    {
        /// <summary>
        /// The rule Id.
        /// </summary>
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int RuleId { get; set; }

        /// <summary>
        /// The name of the approval rule.
        /// </summary>
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [DataMember(Name = "approvals_required")]
        [JsonPropertyName("approvals_required")]
        public int ApprovalsRequired { get; set; }

        /// <summary>
        /// The users as approvers.
        /// </summary>
        [DataMember(Name = "users")]
        [JsonPropertyName("users")]
        public User[] Users { get; set; }

        /// <summary>
        /// The groups as approvers.
        /// </summary>
        [DataMember(Name = "groups")]
        [JsonPropertyName("groups")]
        public Group[] Groups { get; set; }

        /// <summary>
        /// The protected branches to scope the rule by.
        /// </summary>
        [DataMember(Name = "protected_branches")]
        [JsonPropertyName("protected_branches")]
        public Branch[] ProtectedBranch { get; set; }
    }
}
