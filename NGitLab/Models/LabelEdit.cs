using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class LabelEdit
    {
        public LabelEdit()
        {
        }

        public LabelEdit(int projectId, Label label)
        {
            Id = projectId;
            Name = label.Name;
        }

        [Required]
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [Required]
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "new_name")]
        [JsonPropertyName("new_name")]
        public string NewName;

        [DataMember(Name = "color")]
        [JsonPropertyName("color")]
        public string Color;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description;
    }
}
