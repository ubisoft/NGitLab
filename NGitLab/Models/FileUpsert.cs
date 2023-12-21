using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class FileUpsert
{
    [Required]
    [JsonPropertyName("path")]
    public string Path;

    [Required]
    [JsonPropertyName("branch")]
    public string Branch;

    [JsonPropertyName("encoding")]
    public string Encoding;

    [Required]
    [JsonPropertyName("content")]
    public string Content;

    /// <summary>
    /// Use this setter to set the content as base 64.
    /// </summary>
    public string RawContent
    {
        set
        {
            Content = Base64Encode(value);
            Encoding = "base64";
        }
    }

    [Required]
    [JsonPropertyName("commit_message")]
    public string CommitMessage;

    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }
}
