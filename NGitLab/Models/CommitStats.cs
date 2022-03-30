using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class CommitStats
    {
        [JsonPropertyName("additions")]
        public int Additions;

        [JsonPropertyName("deletions")]
        public int Deletions;

        [JsonPropertyName("total")]
        public int Total;
    }
}
