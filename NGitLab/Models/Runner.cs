using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DebuggerDisplay("{Description}")]
    [DataContract]
    public class Runner
    {
        public const string Url = "/runners";

        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "active")]
        [JsonPropertyName("active")]
        public bool Active;

        [DataMember(Name = "online")]
        [JsonPropertyName("online")]
        public bool Online;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description;

        [DataMember(Name = "is_shared")]
        [JsonPropertyName("is_shared")]
        public bool IsShared;

        [DataMember(Name = "contacted_at")]
        [JsonPropertyName("contacted_at")]
        public DateTime ContactedAt;

        [DataMember(Name = "projects")]
        [JsonPropertyName("projects")]
        public Project[] Projects;

        [DataMember(Name = "token")]
        [JsonPropertyName("token")]
        public string Token;

        [DataMember(Name = "tag_list")]
        [JsonPropertyName("tag_list")]
        public string[] TagList;

        [DataMember(Name = "string")]
        [JsonPropertyName("string")]
        public string Version;

        [DataMember(Name = "ip_address")]
        [JsonPropertyName("ip_address")]
        public string IpAddress;

        [DataMember(Name = "locked")]
        [JsonPropertyName("locked")]
        public bool Locked;

        [DataMember(Name = "run_untagged")]
        [JsonPropertyName("run_untagged")]
        public bool RunUntagged;
    }
}
