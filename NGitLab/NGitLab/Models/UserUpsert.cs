using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace NGitLab.Models
{
    public class UserUpsert
    {
        [Required]
        public string Email;

        [Required]
        public string Password;

        [Required]
        public string Username;

        [Required]
        public string Name;
        public string Skype;
        public string Linkedin;
        public string Twitter;

        [JsonProperty("website_url")]
        public string WebsiteURL;

        [JsonProperty("projects_limit")]
        public int ProjectsLimit;

        public string Provider;

        public string Bio;

        [JsonProperty("admin")]
        public bool IsAdmin;

        [JsonProperty("can_create_group")]
        public bool CanCreateGroup;
    }
}