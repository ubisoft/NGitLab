using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class SshKeyCreate
    {
        [JsonPropertyName("title")]
        public string Title;

        [JsonPropertyName("key")]
        public string Key;
    }
}
