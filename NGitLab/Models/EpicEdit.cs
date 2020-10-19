using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class EpicEdit
    {
        [Required]
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [Required]
        [DataMember(Name = "epic_iid")]
        public int EpicId { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "labels")]
        public string Labels { get; set; }

        [DataMember(Name = "state_event")]
        public string State { get; set; }
    }
}
