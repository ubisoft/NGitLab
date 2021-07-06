using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseCreate
    {
        /// <summary>
        /// (required) - The name of the release from which the release is build on
        /// </summary>
        [Required]
        [DataMember(Name = "tag_name")]
        public string Name;

        /// <summary>
        /// (required) - Create release using commit SHA, a tag name, or branch name.
        /// </summary>
        [Required]
        [DataMember(Name = "ref")]
        public string Ref;

        /// <summary>
        /// (optional) - Add release notes to the git release and store it in the GitLab database.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description;
    }
}
