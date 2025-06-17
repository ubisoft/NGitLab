namespace NGitLab;

public class GetRawFileRequest
{
    /// <summary>
    /// The branch name, tag or commit to get the file from. Default is the HEAD of the project.
    /// </summary>
    public string Ref { get; init; }

    /// <summary>
    /// Determines if the response should be Git LFS file contents, rather than the pointer
    /// </summary>
    public bool? Lfs { get; init; }
}
