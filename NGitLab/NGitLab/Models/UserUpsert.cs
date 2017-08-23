using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class UserUpsert {
        [Required]
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [Required]
        [DataMember(Name = "password")]
        public string Password { get; set; }

        [Required]
        [DataMember(Name = "username")]
        public string Username { get; set; }

        [Required]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "skype")]
        public string Skype { get; set; }

        [DataMember(Name = "linkedin")]
        public string Linkedin { get; set; }

        [DataMember(Name = "twitter")]
        public string Twitter { get; set; }

        [DataMember(Name = "website_url")]
        public string WebsiteUrl { get; set; }

        [DataMember(Name = "projects_limit")]
        public int ProjectsLimit { get; set; }

        [DataMember(Name = "provider")]
        public string Provider { get; set; }

        [DataMember(Name = "bio")]
        public string Bio { get; set; }

        [DataMember(Name = "admin")]
        public bool IsAdmin { get; set; }

        [DataMember(Name = "can_create_group")]
        public bool CanCreateGroup { get; set; }

        [DataMember(Name = "reset_password")]
        public bool ResetPassword { get; set; }

        [DataMember(Name = "extern_uid")]
        public string ExternUid { get; set; }

        [DataMember(Name = "confirm")]
        public bool Confirm { get; set; }

        [DataMember(Name = "external")]
        public bool External { get; set; }
    }
}