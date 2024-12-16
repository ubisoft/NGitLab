using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

[DebuggerDisplay("{Name}")]
public class User
{
    public const string Url = "/users";

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("avatar_url")]
    public string AvatarURL { get; set; }

    [JsonPropertyName("web_url")]
    public string WebURL { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("bio")]
    public string Bio { get; set; }

    [JsonPropertyName("bio_html")]
    public string BioHtml { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("public_email")]
    public string PublicEmail { get; set; }

    [JsonPropertyName("skype")]
    public string Skype { get; set; }

    [JsonPropertyName("linkedin")]
    public string Linkedin { get; set; }

    [JsonPropertyName("twitter")]
    public string Twitter { get; set; }

    [JsonPropertyName("website_url")]
    public string WebsiteURL { get; set; }

    [JsonPropertyName("organization")]
    public string Organization { get; set; }

    [JsonPropertyName("job_title")]
    public string JobTitle { get; set; }

    [JsonPropertyName("bot")]
    public bool Bot { get; set; }

    [JsonPropertyName("work_information")]
    public string WorkInformation { get; set; }

    [JsonPropertyName("followers")]
    public int Followers { get; set; }

    [JsonPropertyName("following")]
    public int Following { get; set; }

    [JsonPropertyName("last_sign_in_at")]
    public DateTime LastSignIn { get; set; }

    [JsonPropertyName("confirmed_at")]
    public DateTime ConfirmedAt { get; set; }

    [JsonPropertyName("last_activity_on")]
    public DateTime LastActivityOn { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("theme_id")]
    public long ThemeId { get; set; }

    [JsonPropertyName("color_scheme_id")]
    public long ColorSchemeId { get; set; }

    [JsonPropertyName("projects_limit")]
    public int ProjectsLimit { get; set; }

    [JsonPropertyName("current_sign_in_at")]
    public DateTime CurrentSignIn { get; set; }

    [JsonPropertyName("identities")]
    public Identity[] Identities { get; set; }

    [JsonPropertyName("can_create_group")]
    public bool CanCreateGroup { get; set; }

    [JsonPropertyName("can_create_project")]
    public bool CanCreateProject { get; set; }

    [JsonPropertyName("two_factor_enabled")]
    public bool TwoFactorEnabled { get; set; }

    [JsonPropertyName("external")]
    public bool External { get; set; }

    [JsonPropertyName("private_profile")]
    public bool PrivateProfile { get; set; }

    [JsonPropertyName("commit_email")]
    public string CommitEmail { get; set; }

    [JsonPropertyName("shared_runners_minutes_limit")]
    public int SharedRunnersMinutesLimit { get; set; }

    [JsonPropertyName("extra_shared_runners_minutes_limit")]
    public int ExtraSharedRunnersMinutesLimit { get; set; }

    [JsonPropertyName("is_admin")]
    public bool IsAdmin { get; set; }

    [JsonPropertyName("note")]
    public string Note { get; set; }

    [JsonPropertyName("using_license_seat")]
    public bool UsingLicenseSeat { get; set; }

    [JsonPropertyName("is_auditor")]
    public bool IsAuditor { get; set; }

    [JsonPropertyName("provisioned_by_group_id")]
    public long ProvisionedByGroupId { get; set; }
}
