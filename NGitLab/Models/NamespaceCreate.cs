using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class NamespaceCreate
    {
        [Required]
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [Required]
        [DataMember(Name = "path")]
        [JsonPropertyName("path")]
        public string Path;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description;
    }
}
