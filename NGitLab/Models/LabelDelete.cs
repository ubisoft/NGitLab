using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class LabelDelete
    {
        [Required]
        [DataMember(Name = "id")]
        public int Id;

        [Required]
        [DataMember(Name = "name")]
        public string Name;
    }
}
