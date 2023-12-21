namespace NGitLab.Mock.Config;

/// <summary>
/// Describe a file in project repository
/// </summary>
public class GitLabFileDescriptor
{
    /// <summary>
    /// Relative path to file (required)
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// File content
    /// </summary>
    public string Content { get; set; }
}
