using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class UserUpsert
    {
        [Required]
        [DataMember(Name = "email")]
        [JsonPropertyName("email")]
        public string Email;

        [Required]
        [DataMember(Name = "password")]
        [JsonPropertyName("password")]
        public string Password;

        [Required]
        [DataMember(Name = "username")]
        [JsonPropertyName("username")]
        public string Username;

        [Required]
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

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

        [DataMember(Name = "projects_limit")]
        [JsonPropertyName("projects_limit")]
        public int? ProjectsLimit;

        [DataMember(Name = "provider")]
        [JsonPropertyName("provider")]
        public string Provider;

        [DataMember(Name = "extern_uid")]
        [JsonPropertyName("extern_uid")]
        public string ExternalUid;

        [DataMember(Name = "bio")]
        [JsonPropertyName("bio")]
        public string Bio;

        [DataMember(Name = "admin")]
        [JsonPropertyName("admin")]
        public bool? IsAdmin;

        [DataMember(Name = "can_create_group")]
        [JsonPropertyName("can_create_group")]
        public bool? CanCreateGroup;

        [DataMember(Name = "reset_password")]
        [JsonPropertyName("reset_password")]
        public bool? ResetPassword;

        [DataMember(Name = "skip_confirmation")]
        [JsonPropertyName("skip_confirmation")]
        public bool? SkipConfirmation;
    }
}
