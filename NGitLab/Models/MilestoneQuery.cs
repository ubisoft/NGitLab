namespace NGitLab.Models;

/// <summary>
/// Allows to use more advanced GitLab queries for getting milestones
/// </summary>
public class MilestoneQuery
{
    /// <summary>
    /// Return all milestones or just those that are active or closed
    /// </summary>
    public MilestoneState? State { get; set; }

    /// <summary>
    /// Return only milestones with a title or description matching the provided string
    /// </summary>
    public string Search { get; set; }
}
