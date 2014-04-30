using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class UserUpsert
    {
        [Required]
        [DataMember(Name = "email")]
        public string Email;

        [Required]
        [DataMember(Name = "password")]
        public string Password;

        [Required]
        [DataMember(Name = "username")]
        public string Username;

        [Required]
        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "skype")]
        public string Skype;

        [DataMember(Name = "linkedin")]
        public string Linkedin;

        [DataMember(Name = "twitter")]
        public string Twitter;

        [DataMember(Name = "website_url")]
        public string WebsiteURL;

        [DataMember(Name = "projects_limit")]
        public int ProjectsLimit;

        [DataMember(Name = "provider")]
        public string Provider;

        [DataMember(Name = "bio")]
        public string Bio;

        [DataMember(Name = "admin")]
        public bool IsAdmin;

        [DataMember(Name = "can_create_group")]
        public bool CanCreateGroup;
    }
}