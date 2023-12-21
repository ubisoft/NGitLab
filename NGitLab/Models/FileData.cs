using System;
using System.IO;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class FileData
{
    [JsonPropertyName("file_name")]
    public string Name;

    [JsonPropertyName("file_path")]
    public string Path;

    [JsonPropertyName("size")]
    public int Size;

    [JsonPropertyName("encoding")]
    public string Encoding;

    [JsonPropertyName("content")]
    public string Content;

    [JsonPropertyName("ref")]
    public string Ref;

    [JsonPropertyName("blob_id")]
    public string BlobId;

    [JsonPropertyName("commit_id")]
    public string CommitId;

    [JsonPropertyName("last_commit_id")]
    public string LastCommitId;

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
