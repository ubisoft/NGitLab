using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class EnvironmentInfo
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "slug")]
        [JsonPropertyName("slug")]
        public string Slug;

        [DataMember(Name = "external_url")]
        [JsonPropertyName("external_url")]
        public string ExternalUrl;

        [DataMember(Name = "state")]
        [JsonPropertyName("state")]
        public string State;
    }
}
