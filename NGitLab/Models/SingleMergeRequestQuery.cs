namespace NGitLab.Models
{
    public class SingleMergeRequestQuery
    {
        [QueryParameter("include_diverged_commits_count")]
        public bool? IncludeDivergedCommitsCount { get; set; }

        [QueryParameter("include_rebase_in_progress")]
        public bool? IncludeRebaseInProgress { get; set; }

        [QueryParameter("render_html")]
        public bool? RenderHtml { get; set; }
    }
}
