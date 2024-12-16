using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Namespace
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("kind")]
    public string Kind { get; set; }

    [JsonPropertyName("full_path")]
    public string FullPath { get; set; }

    public enum Type
    {
        Group,
        User,
    }

    public Type GetKind()
    {
        return (Type)Enum.Parse(typeof(Type), Kind, ignoreCase: true);
    }
}
