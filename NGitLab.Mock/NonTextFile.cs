namespace NGitLab.Mock;

public sealed class NonTextFile : File
{
    public NonTextFile(string path, byte[] content)
        : base(path)
    {
        Content = content;
    }

    public override byte[] Content { get; }
}
