namespace NGitLab.Models;

public sealed class ProjectGroupsQuery
{
    /// <summary>
    /// Return list of authorized groups matching the search criteria
    /// </summary>
    public string Search { get; set; }

    /// <summary>
    /// Include groups shared to this project. Default is false
    /// </summary>
    public bool? WithShared { get; set; }

    /// <summary>
    /// Include projects in subgroups of this group. Default is false
    /// </summary>
    public bool? SharedVisibleOnly { get; set; }

    /// <summary>
    /// Limit to shared groups with at least this access level
    /// </summary>
    public AccessLevel? SharedMinAccessLevel { get; set; }

    /// <summary>
    /// Skip the group IDs passed
    /// </summary>
    public long[] SkipGroups { get; set; }
}
