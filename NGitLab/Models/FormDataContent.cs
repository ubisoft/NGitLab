using System.IO;

namespace NGitLab.Models;

public sealed class FormDataContent
{
    public FormDataContent(Stream stream, string name)
    {
        Stream = stream;
        Name = name;
    }

    // <summary>
    // The stream to be uploaded
    // </summary>
    public Stream Stream { get; }

    // <summary>
    // The name of the file being uploaded
    // </summary>
    public string Name { get; }
}
