using System.ComponentModel.DataAnnotations;

namespace NGitLab.Models
{
    public class FileDelete
    {
        [Required]
        public string Path;

        [Required]
        public string Branch;

        [Required]
        public string CommitMessage;
    }
}
