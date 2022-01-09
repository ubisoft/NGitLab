using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class CommitStatus
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int ProjectId;

        [DataMember(Name = "sha")]
        [JsonPropertyName("sha")]
        public string CommitSha;

        [DataMember(Name = "ref")]
        [JsonPropertyName("ref")]
        public string Ref;

        [DataMember(Name = "status")]
        [JsonPropertyName("status")]
        public string Status;

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;
    }
}
