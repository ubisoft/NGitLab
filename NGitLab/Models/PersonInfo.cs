using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class PersonInfo
{
    [JsonPropertyName("name")]
    public string Name;

    [JsonPropertyName("email")]
    public string Email;
}
