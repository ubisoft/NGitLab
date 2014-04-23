using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    public class User
    {
        public static string Url = "/users";

        public int Id;
        public string Username;
        public string Email;
        public string Name;
        public string Skype;
        public string Linkedin;
        public string Twitter;
        public string Provider;
        public string State;
        public bool Blocked;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        public string Bio;

        [DataMember(Name = "dark_scheme")]
        public bool DarkScheme;

        [DataMember(Name = "theme_id")]
        public int ThemeId;

        [DataMember(Name = "extern_uid")]
        public string ExternUid;

        [DataMember(Name = "is_admin")]
        public bool IsAdmin;

        [DataMember(Name = "can_create_group")]
        public bool CanCreateGroup;

        [DataMember(Name = "can_create_project")]
        public bool CanCreateProject;

        [DataMember(Name = "can_create_team")]
        public bool CanCreateTeam;
    }
}