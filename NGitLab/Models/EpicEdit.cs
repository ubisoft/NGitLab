using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class EpicEdit
    {
        [Required]
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [DataMember(Name = "epic_iid")]
        [JsonPropertyName("epic_iid")]
        public int EpicId { get; set; }

        [DataMember(Name = "title")]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [DataMember(Name = "labels")]
        [JsonPropertyName("labels")]
        public string Labels { get; set; }

        [DataMember(Name = "state_event")]
        [JsonPropertyName("state_event")]
        public string State { get; set; }
    }
}
