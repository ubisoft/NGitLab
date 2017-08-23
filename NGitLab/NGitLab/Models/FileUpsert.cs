using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class FileUpsert {
        [Required]
        [DataMember(Name = "file_path")]
        public string Path { get; set; }

        [Required]
        [DataMember(Name = "branch_name")]
        public string Branch { get; set; }

        [DataMember(Name = "encoding")]
        public string Encoding { get; set; }

        [Required]
        [DataMember(Name = "content")]
        public string Content { get; set; }

        [Required]
        [DataMember(Name = "commit_message")]
        public string CommitMessage { get; set; }
    }
}