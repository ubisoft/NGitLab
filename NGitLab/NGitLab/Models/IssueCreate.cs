using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace NGitLab.Models
{
    [DataContract]
    public class IssueCreate
    {


        [Required]
        [DataMember(Name = "id")]
        public int Id;
        
        [Required]
        [DataMember(Name = "title")]
        public string Title;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "assignee_id")]
        public int? AssigneeId;

        [DataMember(Name = "milestone_id")]
        public int? MileStoneId;

        [DataMember(Name = "labels")]
        public string Labels;
    }
}
