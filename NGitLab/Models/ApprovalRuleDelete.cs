using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ApprovalRuleDelete
    {
        /// <summary>
        /// The id of a project.
        /// </summary>
        [Required]
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// The Id of a approval rule.
        /// </summary>
        [Required]
        [DataMember(Name = "approval_rule_id")]
        public int ApprovalRuleId { get; set; }
    }
}
