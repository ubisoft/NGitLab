namespace NGitLab;

public class GetArchiveRequest
{
    /// <summary>
    /// Commit SHA to download. Accepts a tag, branch reference, or SHA. If not specified, defaults to the tip of the default branch.
    /// </summary>
    public string Sha { get; init; }

    /// <summary>
    /// Optional suffix for the archive format, and defaults to tar.gz. For all formats <see cref="ArchiveFormats"/>.
    /// </summary>
    public string Format { get; init; }

    /// <summary>
    /// If true, LFS objects are included in the archive. When set to false, LFS objects are excluded. Default is true.
    /// </summary>
    public bool? IncludeLfsBlobs { get; init; }

    /// <summary>
    /// Subpath of the repository to download. If an empty string, defaults to the whole repository.
    /// </summary>
    public string Path { get; init; }
}
