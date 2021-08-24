using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Identity
    {
        [DataMember(Name = "provider")]
        public string Provider;

        [DataMember(Name = "extern_uid")]
        public string ExternUid;

        [DataMember(Name = "saml_provider_id")]
        public int? SamlProviderId;
    }
}
