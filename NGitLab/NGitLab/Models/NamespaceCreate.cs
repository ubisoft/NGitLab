using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class NamespaceCreate
    {
        [Required]
        [DataMember(Name = "name")]
        public string Name;

        [Required]
		[DataMember(Name = "path")]
		public string Path;

        [DataMember(Name = "description")]
        public string Description;
    }
}