using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class PackageLinks
    {
        [JsonPropertyName("web_path")]
        public string WebPath { get; set; }

        [JsonPropertyName("delete_api_path")]
        public string DeleteApiPath { get; set; }
    }
}
