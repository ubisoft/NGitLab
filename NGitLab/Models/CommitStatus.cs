using System.ComponentModel;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class CommitStatus
{
    // This is NOT the 'Project Id', but some kind of (undocumented) 'Commit Status Id'
    [EditorBrowsable(EditorBrowsableState.Never)]
    [JsonIgnore]
    public int ProjectId;

    [JsonPropertyName("id")]
    public int Id { get => ProjectId; set => ProjectId = value; }

    [JsonPropertyName("sha")]
    public string CommitSha;

    [JsonPropertyName("ref")]
    public string Ref;

    [JsonPropertyName("status")]
    public string Status;

    [JsonPropertyName("name")]
    public string Name;

    [JsonPropertyName("target_url")]
    public string TargetUrl;

    [JsonPropertyName("description")]
    public string Description;

    [JsonPropertyName("coverage")]
    public int? Coverage;

    [JsonPropertyName("author")]
    public Author Author;
}
