using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectIssueNoteEdit
    {
        [JsonIgnore]
        public int IssueId;

        [JsonIgnore]
        public int NoteId;

        [Required]
        [JsonPropertyName("body")]
        public string Body;
    }
}
