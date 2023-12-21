using System.Text;

namespace NGitLab.Mock;

public abstract class File
{
    protected File(string path)
    {
        Path = path;
    }

    public static File CreateFromText(string path, string content)
    {
        return new TextFile(path, content);
    }

    public static File CreateFromText(string path, string content, Encoding encoding)
    {
        return new TextFile(path, content, encoding);
    }

    public string Path { get; }

    public abstract byte[] Content { get; }
}
