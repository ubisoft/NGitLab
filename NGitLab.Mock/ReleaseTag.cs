namespace NGitLab.Mock;

public sealed class ReleaseTag
{
    public string Name { get; }

    public string ReleaseNotes { get; set; }

    public ReleaseTag(string name, string releaseNotes)
    {
        Name = name;
        ReleaseNotes = releaseNotes;
    }
}
