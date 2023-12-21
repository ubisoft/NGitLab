using System;
using System.Text.Json.Serialization;
using NGitLab.Impl.Json;

namespace NGitLab.Models
{
    public class PackageSearchResult
    {
        [JsonPropertyName("id")]
        public int PackageId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("package_type")]
        public PackageType PackageType { get; set; }

        [JsonPropertyName("status")]
        public PackageStatus Status { get; set; }

        [JsonPropertyName("_links")]
        public PackageLinks Links { get; set; }

        [JsonPropertyName("created_at")]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("last_downloaded_at")]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime? LastDownloadedAt { get; set; }
    }
}
