namespace NGitLab;

public class GetRawFileRequest
{
    /// <summary>
    /// The revision of the file to get (name of the branch/tag or commit)
    /// </summary>
    public string Ref { get; set; } = null;

    /// <summary>
    /// Determines if the response should be Git LFS file contents, rather than the pointer
    /// </summary>
    public bool? Lfs { get; set; }
}
