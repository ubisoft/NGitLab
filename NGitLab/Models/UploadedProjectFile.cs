using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public sealed class UploadedProjectFile
    {
        [DataMember(Name = "alt")]
        [JsonPropertyName("alt")]
        public string Alt { get; set; }

        [DataMember(Name = "url")]
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [DataMember(Name = "full_path")]
        [JsonPropertyName("full_path")]
        public string FullPath { get; set; }

        [DataMember(Name = "markdown")]
        [JsonPropertyName("markdown")]
        public string Markdown { get; set; }
    }
}
