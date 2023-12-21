using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class PackageFile
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
