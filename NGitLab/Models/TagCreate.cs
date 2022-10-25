using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class TagCreate
    {
        /// <summary>
        /// (required) - The name of a tag
        /// </summary>
        [Required]
        [JsonPropertyName("tag_name")]
        public string Name;

        /// <summary>
        /// (required) - Create tag using commit SHA, another tag name, or branch name.
        /// </summary>
        [Required]
        [JsonPropertyName("ref")]
        public string Ref;

        /// <summary>
        /// (optional) - Creates annotated tag.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message;

        /// <summary>
        /// (optional) - Add release notes to the git tag and store it in the GitLab database.
        /// </summary>
        [JsonPropertyName("release_description")]
        [Obsolete("Starting in GitLab 14, releases cannot be made through tags. Use `Repository.Releases.Create` instead", false)]
        public string ReleaseDescription;
    }
}
