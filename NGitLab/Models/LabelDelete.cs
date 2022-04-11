using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class LabelDelete
    {
        [Required]
        [JsonPropertyName("id")]
        public int Id;

        [Required]
        [JsonPropertyName("name")]
        public string Name;
    }
}
