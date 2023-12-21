namespace NGitLab.Models;

public class LintCIOptions
{
    /// <summary>
    /// Run pipeline creation simulation, or only do static check.
    /// </summary>
    public bool? DryRun { get; set; }

    /// <summary>
    /// If the list of jobs that would exist in a static check or pipeline simulation should be included in the response.
    /// </summary>
    public bool? IncludeJobs { get; set; }

    /// <summary>
    /// When dry_run is true, sets the branch or tag to use.
    /// </summary>
    public string Ref { get; set; }
}
