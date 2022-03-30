using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
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
    }
}
