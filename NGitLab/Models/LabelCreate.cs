using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class LabelCreate
    {
        [Required]
        public int Id;

        [Required]
        [JsonPropertyName("name")]
        public string Name;

        [Required]
        [JsonPropertyName("color")]
        public string Color;

        [JsonPropertyName("description")]
        public string Description;
    }
}
