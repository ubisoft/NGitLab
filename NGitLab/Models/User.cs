using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DebuggerDisplay("{Name}")]
    public class User
    {
        public const string Url = "/users";

        [JsonPropertyName("id")]
        public int Id;

        [JsonPropertyName("name")]
        public string Name;

        [JsonPropertyName("username")]
        public string Username;

        [JsonPropertyName("state")]
        public string State;

        [JsonPropertyName("avatar_url")]
        public string AvatarURL;

        [JsonPropertyName("web_url")]
        public string WebURL;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [JsonPropertyName("bio")]
        public string Bio;

        [JsonPropertyName("bio_html")]
        public string BioHtml;

        [JsonPropertyName("location")]
        public string Location;

        [JsonPropertyName("public_email")]
        public string PublicEmail;

        [JsonPropertyName("skype")]
        public string Skype;

        [JsonPropertyName("linkedin")]
        public string Linkedin;

        [JsonPropertyName("twitter")]
        public string Twitter;

        [JsonPropertyName("website_url")]
        public string WebsiteURL;

        [JsonPropertyName("organization")]
        public string Organization;

        [JsonPropertyName("job_title")]
        public string JobTitle;

        [JsonPropertyName("bot")]
        public bool Bot;

        [JsonPropertyName("work_information")]
        public string WorkInformation;

        [JsonPropertyName("followers")]
        public int Followers;

        [JsonPropertyName("following")]
        public int Following;

        [JsonPropertyName("last_sign_in_at")]
        public DateTime LastSignIn;

        [JsonPropertyName("confirmed_at")]
        public DateTime ConfirmedAt;

        [JsonPropertyName("last_activity_on")]
        public DateTime LastActivityOn;

        [JsonPropertyName("email")]
        public string Email;

        [JsonPropertyName("theme_id")]
        public int ThemeId;

        [JsonPropertyName("color_scheme_id")]
        public int ColorSchemeId;

        [JsonPropertyName("projects_limit")]
        public int ProjectsLimit;

        [JsonPropertyName("current_sign_in_at")]
        public DateTime CurrentSignIn;

        [JsonPropertyName("identities")]
        public Identity[] Identities;

        [Obsolete("Does not match GitLab's API. Use 'Identities.Provider' instead.")]
        [JsonIgnore]
        public string Provider;

        [Obsolete("Does not match GitLab's API. Use 'Identities.ExternUid' instead.")]
        [JsonIgnore]
        public string ExternUid;

        [Obsolete("Does not match GitLab's API. Use 'State' instead.")]
        [JsonIgnore]
        public bool Blocked;

        [JsonPropertyName("can_create_group")]
        public bool CanCreateGroup;

        [JsonPropertyName("can_create_project")]
        public bool CanCreateProject;

        [JsonPropertyName("two_factor_enabled")]
        public bool TwoFactorEnabled;

        [JsonPropertyName("external")]
        public bool External;

        [JsonPropertyName("private_profile")]
        public bool PrivateProfile;

        [JsonPropertyName("commit_email")]
        public string CommitEmail;

        [JsonPropertyName("shared_runners_minutes_limit")]
        public int SharedRunnersMinutesLimit;

        [JsonPropertyName("extra_shared_runners_minutes_limit")]
        public int ExtraSharedRunnersMinutesLimit;

        [JsonPropertyName("is_admin")]
        public bool IsAdmin;

        [JsonPropertyName("note")]
        public string Note;

        [JsonPropertyName("using_license_seat")]
        public bool UsingLicenseSeat;

        [JsonPropertyName("is_auditor")]
        public bool IsAuditor;

        [JsonPropertyName("provisioned_by_group_id")]
        public int ProvisionedByGroupId;
    }
}
