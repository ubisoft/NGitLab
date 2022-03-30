using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectIssueNoteCreate
    {
        [JsonIgnore]
        public int IssueId;

        [Required]
        [JsonPropertyName("body")]
        public string Body;

        [JsonPropertyName("confidential")]
        public bool Confidential;
    }
}
