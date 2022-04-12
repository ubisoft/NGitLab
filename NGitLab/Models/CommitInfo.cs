using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class CommitInfo
    {
        [JsonPropertyName("id")]
        public Sha1 Id;

        [JsonPropertyName("short_id")]
        public string ShortId;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [JsonPropertyName("parent_ids")]
        public Sha1[] Parents;

        [JsonPropertyName("title")]
        public string Title;

        [JsonPropertyName("message")]
        public string Message;

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

        [JsonPropertyName("web_url")]
        public string WebUrl;
    }
}
