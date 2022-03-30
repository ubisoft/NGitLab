using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class UserUpsert
    {
        [Required]
        [JsonPropertyName("email")]
        public string Email;

        [Required]
        [JsonPropertyName("password")]
        public string Password;

        [Required]
        [JsonPropertyName("username")]
        public string Username;

        [Required]
        [JsonPropertyName("name")]
        public string Name;

        [JsonPropertyName("skype")]
        public string Skype;

        [JsonPropertyName("linkedin")]
        public string Linkedin;

        [JsonPropertyName("twitter")]
        public string Twitter;

        [JsonPropertyName("website_url")]
        public string WebsiteURL;

        [JsonPropertyName("projects_limit")]
        public int? ProjectsLimit;

        [JsonPropertyName("provider")]
        public string Provider;

        [JsonPropertyName("extern_uid")]
        public string ExternalUid;

        [JsonPropertyName("bio")]
        public string Bio;

        [JsonPropertyName("admin")]
        public bool? IsAdmin;

        [JsonPropertyName("can_create_group")]
        public bool? CanCreateGroup;

        [JsonPropertyName("reset_password")]
        public bool? ResetPassword;

        [JsonPropertyName("skip_confirmation")]
        public bool? SkipConfirmation;
    }
}
