using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class NamespaceCreate {
        [Required]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [Required]
        [DataMember(Name = "path")]
        public string Path { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}