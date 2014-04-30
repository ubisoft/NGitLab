using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

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

        [DataMember(Name="website_url")]
        public string WebsiteURL;

        [DataMember(Name="projects_limit")]
        public int ProjectsLimit;

        public string Provider;

        public string Bio;

        [DataMember(Name="admin")]
        public bool IsAdmin;

        [DataMember(Name="can_create_group")]
        public bool CanCreateGroup;
    }
}