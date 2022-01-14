using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class FileDelete
    {
        [Required]
        public string Path;

        [Required]
        [DataMember(Name = "branch")]
        public string Branch;

        [Required]
        [DataMember(Name = "commit_message")]
        public string CommitMessage;
    }
}
