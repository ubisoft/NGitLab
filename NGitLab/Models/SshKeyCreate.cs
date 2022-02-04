using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class SshKeyCreate
    {
        [DataMember(Name = "title")]
        [JsonPropertyName("title")]
        public string Title;

        [DataMember(Name = "key")]
        [JsonPropertyName("key")]
        public string Key;
    }
}
