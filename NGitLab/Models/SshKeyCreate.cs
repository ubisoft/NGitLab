using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class SshKeyCreate
    {
        [JsonPropertyName("title")]
        public string Title;

        [JsonPropertyName("key")]
        public string Key;
    }
}
