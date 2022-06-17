using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class SnippetCreate
    {
        [Required]
        [JsonPropertyName("title")]
        public string Title;

        [Required]
        [JsonPropertyName("file_name")]
        public string FileName;

        [Required]
        [JsonPropertyName("content")]
        public string Content;

        [JsonPropertyName("description")]
        public string Description;

        [JsonPropertyName("visibility")]
        public VisibilityLevel Visibility;

        /// <summary>
        /// An array of snippet files. Required when updating snippets with multiple files.
        /// </summary>
        [JsonPropertyName("files")]
        public SnippetCreateFile[] Files { get; set; }
    }
}
