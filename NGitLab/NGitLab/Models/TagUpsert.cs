using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class TagUpsert
    {
        /// <summary>
        /// (required) - The name of a tag
        /// </summary>
        [DataMember(Name = "tag_name")]
        public string Name;

        /// <summary>
        /// (required) - Create tag using commit SHA, another tag name, or branch name.
        /// </summary>
        [DataMember(Name = "ref")]
        public string Ref;

        /// <summary>
        /// (optional) - Creates annotated tag.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message;

        /// <summary>
        /// (optional) - Add release notes to the git tag and store it in the GitLab database.
        /// </summary>
        [DataMember(Name = "release_description")]
        public string ReleaseDescription;
    }
}