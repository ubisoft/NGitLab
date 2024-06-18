namespace NGitLab.Mock;

public sealed class OtherFile : File
{
    public OtherFile(string path, byte[] content)
        : base(path)
    {
        Content = content;
    }

    public override byte[] Content { get; }
}
