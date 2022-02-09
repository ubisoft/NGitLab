using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class LabelCreate
    {
        [Required]
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [Required]
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [Required]
        [DataMember(Name = "color")]
        [JsonPropertyName("color")]
        public string Color;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description;
    }
}
