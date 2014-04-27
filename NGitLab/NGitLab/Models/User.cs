using System;
using Newtonsoft.Json;

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

        [JsonProperty("created_at")]
        public DateTime CreatedAt;

        public string Bio;

        [JsonProperty("dark_scheme")]
        public bool DarkScheme;

        [JsonProperty("theme_id")]
        public int ThemeId;

        [JsonProperty("extern_uid")]
        public string ExternUid;

        [JsonProperty("is_admin")]
        public bool IsAdmin;

        [JsonProperty("can_create_group")]
        public bool CanCreateGroup;

        [JsonProperty("can_create_project")]
        public bool CanCreateProject;

        [JsonProperty("can_create_team")]
        public bool CanCreateTeam;
    }
}