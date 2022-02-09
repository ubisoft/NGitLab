using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Tag
    {
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "message")]
        [JsonPropertyName("message")]
        public string Message;

        [DataMember(Name = "commit")]
        [JsonPropertyName("commit")]
        public CommitInfo Commit;

        [DataMember(Name = "release")]
        [JsonPropertyName("release")]
        public ReleaseInfo Release;
    }
}
