using System;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class User {
        public static string Url = "/users";

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "username")]
        public string Username { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "skype")]
        public string Skype { get; set; }

        [DataMember(Name = "linkedin")]
        public string Linkedin { get; set; }

        [DataMember(Name = "twitter")]
        public string Twitter { get; set; }

        [DataMember(Name = "provider")]
        public string Provider { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "blocked")]
        public bool Blocked { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "avatar_url")]
        public string AvatarUrl { get; set; }

        [DataMember(Name = "bio")]
        public string Bio { get; set; }

        [DataMember(Name = "color_scheme_id")]
        public int ColorSchemeId { get; set; }

        [DataMember(Name = "theme_id")]
        public int? ThemeId { get; set; }

        [DataMember(Name = "extern_uid")]
        public string ExternUid { get; set; }

        [DataMember(Name = "website_url")]
        public string WebsiteUrl { get; set; }

        [DataMember(Name = "is_admin")]
        public bool IsAdmin { get; set; }

        [DataMember(Name = "can_create_group")]
        public bool CanCreateGroup { get; set; }

        [DataMember(Name = "can_create_project")]
        public bool CanCreateProject { get; set; }

        [DataMember(Name = "two_factor_enabled")]
        public bool TwoFactorEnabled { get; set; }
    }
}