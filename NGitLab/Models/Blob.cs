using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Blob
{
    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("encoding")]
    public string Encoding { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("sha")]
    public Sha1 Sha { get; set; }
}
