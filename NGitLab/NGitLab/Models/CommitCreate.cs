using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class CommitCreate
    {
        [Required]
        [DataMember(Name = "id")]
        public int ProjectId;

        [Required]
        [DataMember(Name = "branch")]
        public string Branch;

        [DataMember(Name = "commit_message")]
        public string CommitMessage;

        [DataMember(Name = "start_branch")]
        public string StartBranch;

        [DataMember(Name = "author_email")]
        public string AuthorEmail;

        [DataMember(Name = "author_name")]
        public string AuthorName;
        
        [DataMember(Name = "actions")]
        public IList<CreateCommitAction> Actions = new List<CreateCommitAction>();
    }

    [DataContract]
    public class CreateCommitAction
    {
        [DataMember(Name = "action")]
        public string Action { get; set; }

        [DataMember(Name = "file_path")]
        public string FilePath{ get; set; }

        [DataMember(Name = "previous_path")]
        public string PreviousPath { get; set; }

        [DataMember(Name = "content")]
        public string Content { get; set; }

        [DataMember(Name = "encoding")]
        public string Encoding { get; set; }
    }
}
