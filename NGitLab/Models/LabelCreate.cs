using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class LabelCreate
    {
        [Obsolete("Use " + nameof(GroupOrProjectId) + " instead")]
        [JsonIgnore]
        public int Id;

        [JsonIgnore]
#pragma warning disable CS0618 // Type or member is obsolete
        public int GroupOrProjectId { get => Id; set => Id = value; }
#pragma warning restore CS0618 // Type or member is obsolete

        [Required]
        [JsonPropertyName("name")]
        public string Name;

        [Required]
        [JsonPropertyName("color")]
        public string Color;

        [JsonPropertyName("description")]
        public string Description;
    }
}
