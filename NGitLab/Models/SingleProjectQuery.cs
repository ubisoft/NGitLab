namespace NGitLab.Models;

/// <summary>
/// Allows to use more advanced GitLab queries for getting projects.
/// </summary>
public class SingleProjectQuery
{
    /// <summary>
    /// Include project statistics
    /// </summary>
    public bool? Statistics { get; set; }
}
