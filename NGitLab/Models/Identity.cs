using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Identity
{
    [JsonPropertyName("provider")]
    public string Provider { get; set; }

    [JsonPropertyName("extern_uid")]
    public string ExternUid { get; set; }

    [JsonPropertyName("saml_provider_id")]
    public long? SamlProviderId { get; set; }
}
