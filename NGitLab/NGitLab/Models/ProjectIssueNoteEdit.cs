using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectIssueNoteEdit
    {
        public int IssueId;
        public int NoteId;

        [Required]
        [DataMember(Name = "body")]
        public string Body;
    }
}
