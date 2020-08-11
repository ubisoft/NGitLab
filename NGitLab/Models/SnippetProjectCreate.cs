using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class SnippetProjectCreate
    {
        [Required]
        [DataMember(Name = "id")]
        public int ProjectId;

        [Required]
        [DataMember(Name = "title")]
        public string Title;

        [Required]
        [DataMember(Name = "file_name")]
        public string FileName;

        [DataMember(Name = "description")]
        public string Description;

        [Required]
        [DataMember(Name = "content")]
        public string Code;

        [Required]
        [DataMember(Name = "visibility")]
        public VisibilityLevel Visibility;
    }
}
