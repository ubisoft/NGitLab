using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class SshKey
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [DataMember(Name = "title")]
        [JsonPropertyName("title")]
        public string Title;

        [DataMember(Name = "key")]
        [JsonPropertyName("key")]
        public string Key;

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreateAt;
    }
}
