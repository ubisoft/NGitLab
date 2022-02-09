using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Contributor
    {
        public const string Url = "/contributors";

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "email")]
        [JsonPropertyName("email")]
        public string Email;

        [DataMember(Name = "commits")]
        [JsonPropertyName("commits")]
        public int Commits;

        [DataMember(Name = "additions")]
        [JsonPropertyName("additions")]
        public int Addition;

        [DataMember(Name = "deletions")]
        [JsonPropertyName("deletions")]
        public int Deletions;
    }
}
