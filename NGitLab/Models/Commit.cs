using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Commit
    {
        public const string Url = "/commits";

        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public Sha1 Id;

        [DataMember(Name = "title")]
        [JsonPropertyName("title")]
        public string Title;

        [DataMember(Name = "short_id")]
        [JsonPropertyName("short_id")]
        public string ShortId;

        [DataMember(Name = "author_name")]
        [JsonPropertyName("author_name")]
        public string AuthorName;

        [DataMember(Name = "author_email")]
        [JsonPropertyName("author_email")]
        public string AuthorEmail;

        [DataMember(Name = "authored_date")]
        [JsonPropertyName("authored_date")]
        public DateTime AuthoredDate;

        [DataMember(Name = "committer_name")]
        [JsonPropertyName("committer_name")]
        public string CommitterName;

        [DataMember(Name = "committer_email")]
        [JsonPropertyName("committer_email")]
        public string CommitterEmail;

        [DataMember(Name = "committed_date")]
        [JsonPropertyName("committed_date")]
        public DateTime CommittedDate;

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "message")]
        [JsonPropertyName("message")]
        public string Message;

        [DataMember(Name = "parent_ids")]
        [JsonPropertyName("parent_ids")]
        public Sha1[] Parents;

        [DataMember(Name = "status")]
        [JsonPropertyName("status")]
        public string Status;

        [DataMember(Name = "stats")]
        [JsonPropertyName("stats")]
        public CommitStats Stats;

        [DataMember(Name = "web_url")]
        [JsonPropertyName("web_url")]
        public string WebUrl;
    }
}
