using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Author : IEquatable<Author>
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; }

    [JsonPropertyName("web_url")]
    public string WebUrl { get; set; }

    public bool Equals(Author other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        // Compare IDs only
        return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Author);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
