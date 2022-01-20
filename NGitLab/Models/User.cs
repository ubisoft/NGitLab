using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    [DebuggerDisplay("{Name}")]
    public class User
    {
        public const string Url = "/users";

        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "username")]
        [JsonPropertyName("username")]
        public string Username;

        [DataMember(Name = "state")]
        [JsonPropertyName("state")]
        public string State;

        [DataMember(Name = "avatar_url")]
        [JsonPropertyName("avatar_url")]
        public string AvatarURL;

        [DataMember(Name = "web_url")]
        [JsonPropertyName("web_url")]
        public string WebURL;

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "bio")]
        [JsonPropertyName("bio")]
        public string Bio;

        [DataMember(Name = "bio_html")]
        [JsonPropertyName("bio_html")]
        public string BioHtml;

        [DataMember(Name = "location")]
        [JsonPropertyName("location")]
        public string Location;

        [DataMember(Name = "public_email")]
        [JsonPropertyName("public_email")]
        public string PublicEmail;

        [DataMember(Name = "skype")]
        [JsonPropertyName("skype")]
        public string Skype;

        [DataMember(Name = "linkedin")]
        [JsonPropertyName("linkedin")]
        public string Linkedin;

        [DataMember(Name = "twitter")]
        [JsonPropertyName("twitter")]
        public string Twitter;

        [DataMember(Name = "website_url")]
        [JsonPropertyName("website_url")]
        public string WebsiteURL;

        [DataMember(Name = "organization")]
        [JsonPropertyName("organization")]
        public string Organization;

        [DataMember(Name = "job_title")]
        [JsonPropertyName("job_title")]
        public string JobTitle;

        [DataMember(Name = "bot")]
        [JsonPropertyName("bot")]
        public bool Bot;

        [DataMember(Name = "work_information")]
        [JsonPropertyName("work_information")]
        public string WorkInformation;

        [DataMember(Name = "followers")]
        [JsonPropertyName("followers")]
        public int Followers;

        [DataMember(Name = "following")]
        [JsonPropertyName("following")]
        public int Following;

        [DataMember(Name = "last_sign_in_at")]
        [JsonPropertyName("last_sign_in_at")]
        public DateTime LastSignIn;

        [DataMember(Name = "confirmed_at")]
        [JsonPropertyName("confirmed_at")]
        public DateTime ConfirmedAt;

        [DataMember(Name = "last_activity_on")]
        [JsonPropertyName("last_activity_on")]
        public DateTime LastActivityOn;

        [DataMember(Name = "email")]
        [JsonPropertyName("email")]
        public string Email;

        [DataMember(Name = "theme_id")]
        [JsonPropertyName("theme_id")]
        public int ThemeId;

        [DataMember(Name = "color_scheme_id")]
        [JsonPropertyName("color_scheme_id")]
        public int ColorSchemeId;

        [DataMember(Name = "projects_limit")]
        [JsonPropertyName("projects_limit")]
        public int ProjectsLimit;

        [DataMember(Name = "current_sign_in_at")]
        [JsonPropertyName("current_sign_in_at")]
        public DateTime CurrentSignIn;

        [DataMember(Name = "identities")]
        [JsonPropertyName("identities")]
        public Identity[] Identities;

        [Obsolete("This does not match GitLab's Api. Use Identities.Provider instead.")]
        [DataMember(Name = "provider")]
        [JsonPropertyName("provider")]
        public string Provider;

        [Obsolete("This does not match GitLab's Api. Use Identities.ExternUid instead.")]
        [DataMember(Name = "extern_uid")]
        [JsonPropertyName("extern_uid")]
        public string ExternUid;

        [DataMember(Name = "blocked")]
        [JsonPropertyName("blocked")]
        public bool Blocked;

        [DataMember(Name = "can_create_group")]
        [JsonPropertyName("can_create_group")]
        public bool CanCreateGroup;

        [DataMember(Name = "can_create_project")]
        [JsonPropertyName("can_create_project")]
        public bool CanCreateProject;

        [DataMember(Name = "two_factor_enabled")]
        [JsonPropertyName("two_factor_enabled")]
        public bool TwoFactorEnabled;

        [DataMember(Name = "external")]
        [JsonPropertyName("external")]
        public bool External;

        [DataMember(Name = "private_profile")]
        [JsonPropertyName("private_profile")]
        public bool PrivateProfile;

        [DataMember(Name = "commit_email")]
        [JsonPropertyName("commit_email")]
        public string CommitEmail;

        [DataMember(Name = "shared_runners_minutes_limit")]
        [JsonPropertyName("shared_runners_minutes_limit")]
        public int SharedRunnersMinutesLimit;

        [DataMember(Name = "extra_shared_runners_minutes_limit")]
        [JsonPropertyName("extra_shared_runners_minutes_limit")]
        public int ExtraSharedRunnersMinutesLimit;

        [DataMember(Name = "is_admin")]
        [JsonPropertyName("is_admin")]
        public bool IsAdmin;

        [DataMember(Name = "note")]
        [JsonPropertyName("note")]
        public string Note;

        [DataMember(Name = "using_license_seat")]
        [JsonPropertyName("using_license_seat")]
        public bool UsingLicenseSeat;

        [DataMember(Name = "is_auditor")]
        [JsonPropertyName("is_auditor")]
        public bool IsAuditor;

        [DataMember(Name = "provisioned_by_group_id")]
        [JsonPropertyName("provisioned_by_group_id")]
        public int ProvisionedByGroupId;
    }
}
