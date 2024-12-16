using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Commit
{
    public const string Url = "/commits";

    [JsonPropertyName("id")]
    public Sha1 Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("short_id")]
    public string ShortId { get; set; }

    [JsonPropertyName("author_name")]
    public string AuthorName { get; set; }

    [JsonPropertyName("author_email")]
    public string AuthorEmail { get; set; }

    [JsonPropertyName("authored_date")]
    public DateTime AuthoredDate { get; set; }

    [JsonPropertyName("committer_name")]
    public string CommitterName { get; set; }

    [JsonPropertyName("committer_email")]
    public string CommitterEmail { get; set; }

    [JsonPropertyName("committed_date")]
    public DateTime CommittedDate { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("parent_ids")]
    public Sha1[] Parents { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("stats")]
    public CommitStats Stats { get; set; }

    [JsonPropertyName("web_url")]
    public string WebUrl { get; set; }

    [JsonPropertyName("last_pipeline")]
    public Pipeline LastPipeline { get; set; }
}
