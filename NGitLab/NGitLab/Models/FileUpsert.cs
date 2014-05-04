using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class FileUpsert
    {
        [Required]
        [DataMember(Name="file_path")]
        public string Path; 
        
        [Required]
        [DataMember(Name="branch_name")]
        public string Branch;

        [DataMember(Name="encoding")]
        public string Encoding;

        [Required]
        [DataMember(Name="content")]
        public string Content;
        
        [Required]
        [DataMember(Name="commit_message")]
        public string CommitMessage;
    }
}