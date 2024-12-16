using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Session : User
{
    public new const string Url = "/session";

    [JsonPropertyName("private_token")]
    public string PrivateToken { get; set; }
}
