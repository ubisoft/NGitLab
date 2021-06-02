using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class PipelineCreate
    {
        [Required]
        [DataMember(Name = "ref")]
        public string Ref { get; set; }

        [DataMember(Name = "variables")]
        public IDictionary<string, string> Variables { get; } = new Dictionary<string, string>(StringComparer.Ordinal);
    }
}
