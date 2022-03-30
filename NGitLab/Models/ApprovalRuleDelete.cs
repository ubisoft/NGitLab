using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ApprovalRuleDelete
    {
        /// <summary>
        /// The id of a project.
        /// </summary>
        [Required]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The Id of a approval rule.
        /// </summary>
        [Required]
        [JsonPropertyName("approval_rule_id")]
        public int ApprovalRuleId { get; set; }
    }
}
