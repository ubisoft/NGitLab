using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Identity
    {
        [JsonPropertyName("provider")]
        public string Provider;

        [JsonPropertyName("extern_uid")]
        public string ExternUid;

        [JsonPropertyName("saml_provider_id")]
        public int? SamlProviderId;
    }
}
