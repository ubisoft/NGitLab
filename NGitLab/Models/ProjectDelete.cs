namespace NGitLab.Models;

public sealed class ProjectDelete
{
    public bool? PermanentlyRemove { get; set; }

    public string FullPath { get; set; }
}
