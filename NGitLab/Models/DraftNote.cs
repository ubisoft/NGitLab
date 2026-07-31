using System.Text.Json.Serialization;

namespace NGitLab.Models;

/// <summary>
/// A draft (unpublished) note on a merge request.
/// </summary>
public class DraftNote
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("author_id")]
    public long AuthorId { get; set; }

    [JsonPropertyName("merge_request_id")]
    public long MergeRequestId { get; set; }

    [JsonPropertyName("note")]
    public string Note { get; set; }

    [JsonPropertyName("position")]
    public Position Position { get; set; }
}

/// <summary>
/// Payload for creating a draft note on a merge request.
/// </summary>
public class DraftNoteCreate
{
    [JsonPropertyName("note")]
    public string Note { get; set; }

    /// <summary>
    /// Required for inline (diff) draft notes; omit for general notes.
    /// </summary>
    [JsonPropertyName("position")]
    public Position Position { get; set; }
}
