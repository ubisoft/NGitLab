using System.ComponentModel.DataAnnotations;

namespace NGitLab.Models
{
    public class FileDelete
    {
        [Required]
        public string Path;

        [Required]
        [QueryParameter("branch")]
        public string Branch;

        [Required]
        [QueryParameter("commit_message")]
        public string CommitMessage;
    }
}
