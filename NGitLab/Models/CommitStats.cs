using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class CommitStats
    {
        [DataMember(Name = "additions")]
        [JsonPropertyName("additions")]
        public int Additions;

        [DataMember(Name = "deletions")]
        [JsonPropertyName("deletions")]
        public int Deletions;

        [DataMember(Name = "total")]
        [JsonPropertyName("total")]
        public int Total;
    }
}
