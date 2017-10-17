using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NGitLab.Models
{
    [DataContract]
    public class SnippetCreate
    {
        [Required]
        [DataMember(Name = "title")]
        public string Title;

        [Required]
        [DataMember(Name = "file_name")]
        public string FileName;

        [Required]
        [DataMember(Name = "content")]
        public string Content;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "visibility")]
        public VisibilityLevel Visibility;
    }
}
