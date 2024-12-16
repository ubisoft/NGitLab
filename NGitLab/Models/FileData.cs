using System;
using System.IO;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class FileData
{
    [JsonPropertyName("file_name")]
    public string Name { get; set; }

    [JsonPropertyName("file_path")]
    public string Path { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("encoding")]
    public string Encoding { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("ref")]
    public string Ref { get; set; }

    [JsonPropertyName("blob_id")]
    public string BlobId { get; set; }

    [JsonPropertyName("commit_id")]
    public string CommitId { get; set; }

    [JsonPropertyName("last_commit_id")]
    public string LastCommitId { get; set; }

    public string DecodedContent
    {
        get
        {
            if (string.Equals(Encoding, "base64", StringComparison.Ordinal))
                return Base64Decode(Content);
            return Content;
        }
    }

    public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        using var ms = new MemoryStream(base64EncodedBytes);
        using var streamReader = new StreamReader(ms, detectEncodingFromByteOrderMarks: true);
        return streamReader.ReadToEnd();
    }
}
