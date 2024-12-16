using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class UserUpsert
{
    [Required]
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [Required]
    [JsonPropertyName("password")]
    public string Password { get; set; }

    [Required]
    [JsonPropertyName("username")]
    public string Username { get; set; }

    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("skype")]
    public string Skype { get; set; }

    [JsonPropertyName("linkedin")]
    public string Linkedin { get; set; }

    [JsonPropertyName("twitter")]
    public string Twitter { get; set; }

    [JsonPropertyName("website_url")]
    public string WebsiteURL { get; set; }

    [JsonPropertyName("projects_limit")]
    public int? ProjectsLimit { get; set; }

    [JsonPropertyName("provider")]
    public string Provider { get; set; }

    [JsonPropertyName("extern_uid")]
    public string ExternalUid { get; set; }

    [JsonPropertyName("bio")]
    public string Bio { get; set; }

    [JsonPropertyName("admin")]
    public bool? IsAdmin { get; set; }

    [JsonPropertyName("can_create_group")]
    public bool? CanCreateGroup { get; set; }

    [JsonPropertyName("reset_password")]
    public bool? ResetPassword { get; set; }

    [JsonPropertyName("skip_confirmation")]
    public bool? SkipConfirmation { get; set; }

    [JsonPropertyName("private_profile")]
    public bool? IsPrivateProfile { get; set; }
}
