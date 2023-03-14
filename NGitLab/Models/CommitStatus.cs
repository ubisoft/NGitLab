using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class CommitStatus
    {
        [Obsolete("This is NOT the 'Project Id' (could be some kind of 'Commit Status Id')")]
        [JsonPropertyName("id")]
        public int ProjectId;

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
