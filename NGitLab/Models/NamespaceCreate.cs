using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class NamespaceCreate
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name;

        [Required]
        [JsonPropertyName("path")]
        public string Path;

        [JsonPropertyName("description")]
        public string Description;
    }
}
