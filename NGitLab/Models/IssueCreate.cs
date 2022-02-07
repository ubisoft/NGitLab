using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class IssueCreate
    {
        [Required]
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [Required]
        [DataMember(Name = "title")]
        [JsonPropertyName("title")]
        public string Title;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description;

        [DataMember(Name = "assignee_id")]
        [JsonPropertyName("assignee_id")]
        public int? AssigneeId;

        [DataMember(Name = "milestone_id")]
        [JsonPropertyName("milestone_id")]
        public int? MileStoneId;

        [DataMember(Name = "labels")]
        [JsonPropertyName("labels")]
        public string Labels;

        [DataMember(Name = "confidential")]
        [JsonPropertyName("confidential")]
        public bool Confidential;
    }
}
