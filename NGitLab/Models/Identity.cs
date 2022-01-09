using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Identity
    {
        [DataMember(Name = "provider")]
        [JsonPropertyName("provider")]
        public string Provider;

        [DataMember(Name = "extern_uid")]
        [JsonPropertyName("extern_uid")]
        public string ExternUid;

        [DataMember(Name = "saml_provider_id")]
        [JsonPropertyName("saml_provider_id")]
        public int? SamlProviderId;
    }
}
