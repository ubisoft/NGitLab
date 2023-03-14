using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class EpicEdit
    {
        [Obsolete("'Project Id' is ignored in the PUT JSON; it is actually specified through the endpoint URL.")]
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("epic_iid")]
        public int EpicId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("labels")]
        public string Labels { get; set; }

        [JsonPropertyName("state_event")]
        public string State { get; set; }
    }
}
