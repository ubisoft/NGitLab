using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class LabelDelete
    {
        [Required]
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [Required]
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;
    }
}
