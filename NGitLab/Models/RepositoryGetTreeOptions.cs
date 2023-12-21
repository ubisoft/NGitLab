namespace NGitLab.Models;

public class RepositoryGetTreeOptions
{
    public string Path { get; set; }

    public string Ref { get; set; }

    public bool Recursive { get; set; }

    public uint? PerPage { get; set; }
}
