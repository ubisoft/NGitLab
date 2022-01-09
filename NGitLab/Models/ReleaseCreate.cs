using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseCreate
    {
        /// <summary>
        /// (required) - The tag where the release is created from.
        /// </summary>
        [Required]
        [DataMember(Name = "tag_name")]
        [JsonPropertyName("tag_name")]
        public string TagName { get; set; }

        /// <summary>
        /// (optional) - The description of the release.
        /// </summary>
        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// (optional) - The release name.
        /// </summary>
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        ///  - Required if tag_name doesn't exist. It can be a commit SHA, a tag name, or a branch name.
        /// </summary>
        [DataMember(Name = "ref")]
        [JsonPropertyName("ref")]
        public string Ref { get; set; }

        /// <summary>
        ///  - The title of each milestone the release is associated with.
        /// </summary>
        [DataMember(Name = "milestones")]
        [JsonPropertyName("milestones")]
        public string[] Milestones { get; set; }

        /// <summary>
        ///  - Assets containing an array of links.
        /// </summary>
        [DataMember(Name = "assets")]
        [JsonPropertyName("assets")]
        public ReleaseAssetsInfo Assets { get; set; }

        /// <summary>
        ///  - The date when the release is/was ready. Defaults to the current time.
        /// </summary>
        [DataMember(Name = "released_at")]
        [JsonPropertyName("released_at")]
        public DateTime? ReleasedAt { get; set; }
    }
}
