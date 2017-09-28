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
        [DataMember(Name = "code")]
        public string Code;

        [Required]
        [DataMember(Name = "visibility")]
        public VisibilityLevel Visibility;
    }
}
