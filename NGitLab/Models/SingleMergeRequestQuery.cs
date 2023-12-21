using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class SingleMergeRequestQuery
{
    [JsonPropertyName("include_diverged_commits_count")]
    public bool? IncludeDivergedCommitsCount { get; set; }

    [JsonPropertyName("include_rebase_in_progress")]
    public bool? IncludeRebaseInProgress { get; set; }

    [JsonPropertyName("render_html")]
    public bool? RenderHtml { get; set; }
}
