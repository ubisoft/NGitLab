using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class SnippetCreateFile
    {
        /// <summary>
        /// Type of action to perform on the file, one of: create, update, delete, move
        /// </summary>
        [JsonPropertyName("action")]
        public string Action { get; set; }

        /// <summary>
        /// File path of the snippet file
        /// </summary>
        [JsonPropertyName("file_path")]
        public string FilePath { get; set; }

        /// <summary>
        /// Previous path of the snippet file
        /// </summary>
        [JsonPropertyName("previous_path")]
        public string PreviousFile { get; set; }

        /// <summary>
        /// Content of the snippet file
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
