using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class CommitStatus
    {
        [Obsolete("This is NOT the 'Project Id' (was some kind of 'Commit Status Id')")]
        [JsonIgnore]
        public int ProjectId;

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("sha")]
        public string CommitSha;

        [JsonPropertyName("ref")]
        public string Ref;

        [JsonPropertyName("status")]
        public string Status;

        [JsonPropertyName("name")]
        public string Name;
    }
}
