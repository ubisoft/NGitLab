using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class SnippetProjectCreate
    {
        [Required]
        [JsonPropertyName("id")]
        public int ProjectId;

        [Required]
        [JsonPropertyName("title")]
        public string Title;

        [Required]
        [JsonPropertyName("file_name")]
        public string FileName;

        [JsonPropertyName("description")]
        public string Description;

        [Required]
        [JsonPropertyName("content")]
        public string Code;

        [Required]
        [JsonPropertyName("visibility")]
        public VisibilityLevel Visibility;
    }
}
