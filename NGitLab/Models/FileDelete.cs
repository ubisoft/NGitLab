using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class FileDelete
    {
        [Required]
        public string Path;

        [Required]
        [DataMember(Name = "branch")]
        [JsonPropertyName("branch")]
        public string Branch;

        [Required]
        [DataMember(Name = "commit_message")]
        [JsonPropertyName("commit_message")]
        public string CommitMessage;
    }
}
