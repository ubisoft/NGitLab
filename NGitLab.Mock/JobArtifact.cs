namespace NGitLab.Mock;

public sealed class JobArtifact : GitLabObject
{
    public string Filename { get; set; }

    public long Size { get; set; }
}
