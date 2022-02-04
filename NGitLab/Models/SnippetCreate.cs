using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class SnippetCreate
    {
        [Required]
        [DataMember(Name = "title")]
        [JsonPropertyName("title")]
        public string Title;

        [Required]
        [DataMember(Name = "file_name")]
        [JsonPropertyName("file_name")]
        public string FileName;

        [Required]
        [DataMember(Name = "content")]
        [JsonPropertyName("content")]
        public string Content;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description;

        [DataMember(Name = "visibility")]
        [JsonPropertyName("visibility")]
        public VisibilityLevel Visibility;
    }
}
