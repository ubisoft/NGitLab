using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Commit
    {
        public const string Url = "/commits";

        [JsonPropertyName("id")]
        public Sha1 Id;

        [JsonPropertyName("title")]
        public string Title;

        [JsonPropertyName("short_id")]
        public string ShortId;

        [JsonPropertyName("author_name")]
        public string AuthorName;

        [JsonPropertyName("author_email")]
        public string AuthorEmail;

        [JsonPropertyName("authored_date")]
        public DateTime AuthoredDate;

        [JsonPropertyName("committer_name")]
        public string CommitterName;

        [JsonPropertyName("committer_email")]
        public string CommitterEmail;

        [JsonPropertyName("committed_date")]
        public DateTime CommittedDate;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [JsonPropertyName("message")]
        public string Message;

        [JsonPropertyName("parent_ids")]
        public Sha1[] Parents;

        [JsonPropertyName("status")]
        public string Status;

        [JsonPropertyName("stats")]
        public CommitStats Stats;

        [JsonPropertyName("web_url")]
        public string WebUrl;
    }
}
