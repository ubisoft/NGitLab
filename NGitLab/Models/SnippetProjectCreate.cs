using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class SnippetProjectCreate
    {
        [Required]
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int ProjectId;

        [Required]
        [DataMember(Name = "title")]
        [JsonPropertyName("title")]
        public string Title;

        [Required]
        [DataMember(Name = "file_name")]
        [JsonPropertyName("file_name")]
        public string FileName;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description;

        [Required]
        [DataMember(Name = "content")]
        [JsonPropertyName("content")]
        public string Code;

        [Required]
        [DataMember(Name = "visibility")]
        [JsonPropertyName("visibility")]
        public VisibilityLevel Visibility;
    }
}
