using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract(Name = "ReleaseLinkType")]
    public enum ReleaseLinkType
    {
        [EnumMember(Value = "other")]
        Other,
        [EnumMember(Value = "runbook")]
        Runbook,
        [EnumMember(Value = "image")]
        Image,
        [EnumMember(Value = "package")]
        Package,
    }

    [DataContract]
    public class ReleaseLink
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [DataMember(Name = "url")]
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [DataMember(Name = "direct_asset_url")]
        [JsonPropertyName("direct_asset_url")]
        public string DirectAssetUrl { get; set; }

        [DataMember(Name = "external")]
        [JsonPropertyName("external")]
        public bool External { get; set; }

        [DataMember(Name = "link_type")]
        [JsonPropertyName("link_type")]
        public ReleaseLinkType LinkType { get; set; }
    }
}
