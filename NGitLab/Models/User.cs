using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    [DebuggerDisplay("{Name}")]
    public class User
    {
        public const string Url = "/users";

        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "username")]
        public string Username;

        [DataMember(Name = "state")]
        public string State;

        [DataMember(Name = "avatar_url")]
        public string AvatarURL;

        [DataMember(Name = "web_url")]
        public string WebURL;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "bio")]
        public string Bio;

        [DataMember(Name = "bio_html")]
        public string BioHtml;

        [DataMember(Name = "location")]
        public string Location;

        [DataMember(Name = "public_email")]
        public string PublicEmail;

        [DataMember(Name = "skype")]
        public string Skype;

        [DataMember(Name = "linkedin")]
        public string Linkedin;

        [DataMember(Name = "twitter")]
        public string Twitter;

        [DataMember(Name = "website_url")]
        public string WebsiteURL;

        [DataMember(Name = "organization")]
        public string Organization;

        [DataMember(Name = "job_title")]
        public string JobTitle;

        [DataMember(Name = "bot")]
        public bool Bot;

        [DataMember(Name = "work_information")]
        public string WorkInformation;

        [DataMember(Name = "followers")]
        public int Followers;

        [DataMember(Name = "following")]
        public int Following;

        [DataMember(Name = "last_sign_in_at")]
        public DateTime LastSignIn;

        [DataMember(Name = "confirmed_at")]
        public DateTime ConfirmedAt;

        [DataMember(Name = "last_activity_on")]
        public DateTime LastActivityOn;

        [DataMember(Name = "email")]
        public string Email;

        [DataMember(Name = "theme_id")]
        public int ThemeId;

        [DataMember(Name = "color_scheme_id")]
        public int ColorSchemeId;

        [DataMember(Name = "projects_limit")]
        public int ProjectsLimit;

        [DataMember(Name = "current_sign_in")]
        public DateTime CurrentSignIn;

        [DataMember(Name = "identities")]
        public Identity[] Identities;

        [Obsolete("This does not match GitLab's Api. Use Identities.Provider instead.")]
        [DataMember(Name = "provider")]
        public string Provider;

        [Obsolete("This does not match GitLab's Api. Use Identities.ExternUid instead.")]
        [DataMember(Name = "extern_uid")]
        public string ExternUid;

        [Obsolete("This does not match GitLab's Api. Use State instead")]
        [DataMember(Name = "blocked")]
        public bool Blocked;

        [DataMember(Name = "can_create_group")]
        public bool CanCreateGroup;

        [DataMember(Name = "can_create_project")]
        public bool CanCreateProject;

        [DataMember(Name = "two_factor_enabled")]
        public bool TwoFactorEnabled;

        [DataMember(Name = "external")]
        public bool External;

        [DataMember(Name = "private_profile")]
        public bool PrivateProfile;

        [DataMember(Name = "commit_email")]
        public string CommitEmail;

        [DataMember(Name = "shared_runners_minutes_limit")]
        public int SharedRunnersMinutesLimit;

        [DataMember(Name = "extra_shared_runners_minutes_limit")]
        public int ExtraSharedRunnersMinutesLimit;

        [DataMember(Name = "is_admin")]
        public bool IsAdmin;

        [DataMember(Name = "note")]
        public string Note;

        [DataMember(Name = "using_license_seat")]
        public bool UsingLicenseSeat;

        [DataMember(Name = "is_auditor")]
        public bool IsAuditor;

        [DataMember(Name = "provisioned_by_group_id")]
        public int ProvisionedByGroupId;


    }
}
