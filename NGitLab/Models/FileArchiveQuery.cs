namespace NGitLab.Models;

public sealed class FileArchiveQuery
{
    public FileArchiveFormat? Format { get; set; }

    // This property is named Ref because even though the query string parameter key is 'sha' it accepts any ref
    // i.e. branch name, sha, tag
    public string Ref { get; set; }

    public string Path { get; set; }
}
