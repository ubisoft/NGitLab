using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class PipelineGetJobsQuery
    {
        [Required]
        [DataMember(Name = "id")]
        public int PipelineId { get; set; }

        public string[] Scope { get; set; }

        public bool? IncludeRetried { get; set; }
    }
}
