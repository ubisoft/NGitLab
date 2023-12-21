using System.Text;

namespace NGitLab.Mock;

public sealed class TextFile : File
{
    public TextFile(string path, string content)
        : this(path, content, Encoding.UTF8)
    {
    }

    public TextFile(string path, string content, Encoding encoding)
        : base(path)
    {
        TextContent = content;
        Encoding = encoding;
    }

    public string TextContent { get; }

    public Encoding Encoding { get; }

    public override byte[] Content => Encoding.GetBytes(TextContent);
}
