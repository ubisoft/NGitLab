using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class Tag
    {
        [JsonPropertyName("name")]
        public string Name;

        [JsonPropertyName("message")]
        public string Message;

        [JsonPropertyName("commit")]
        public CommitInfo Commit;

        [JsonPropertyName("release")]
        public ReleaseInfo Release;

        [JsonPropertyName("target")]
        public Sha1 Target { get; set; }

        [JsonPropertyName("protected")]
        public bool Protected { get; set; }
    }
}
