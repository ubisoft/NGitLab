using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectIssueNoteCreate
    {
        public int IssueId;

        [Required]
        [DataMember(Name = "body")]
        public string Body;
    }
}
