using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseUpdate
    {
        /// <summary>
        /// (required) - The Git tag the release is associated with.
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
        ///  - The title of each milestone the release is associated with.
        /// </summary>
        [DataMember(Name = "milestones")]
        [JsonPropertyName("milestones")]
        public string[] Milestones { get; set; }

        /// <summary>
        ///  - The date when the release is/was ready. Defaults to the current time.
        /// </summary>
        [DataMember(Name = "released_at")]
        [JsonPropertyName("released_at")]
        public DateTime? ReleasedAt { get; set; }
    }
}
