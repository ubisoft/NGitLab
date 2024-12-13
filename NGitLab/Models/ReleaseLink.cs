using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

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

public class ReleaseLink
{
    [JsonPropertyName("id")]
    public long? Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("direct_asset_url")]
    public string DirectAssetUrl { get; set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [JsonPropertyName("external")]
    public bool External { get; set; }

    [JsonPropertyName("link_type")]
    public ReleaseLinkType LinkType { get; set; }
}
