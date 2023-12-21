using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class TagProtect
{
    public TagProtect(string name)
    {
        Name = name;
    }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("allowed_to_create")]
    public AccessControl[] AllowedToCreate { get; set; }

    [JsonPropertyName("create_access_level")]
    public AccessLevel? CreateAccessLevel { get; set; }
}
