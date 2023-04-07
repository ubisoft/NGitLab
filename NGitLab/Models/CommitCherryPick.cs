using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class CommitCherryPick
    {
        [Required]
        [JsonIgnore]
        public string Sha;

        [Required]
        [JsonPropertyName("branch")]
        public string Branch;

        [JsonPropertyName("message")]
        public string Message;
    }
}
