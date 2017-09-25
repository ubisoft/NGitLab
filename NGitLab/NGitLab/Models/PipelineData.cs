using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NGitLab.Models {
    public enum PipelineStatus {
        pending,
        building,
        failed,
        success,
        undefined,
    }

    [DataContract]
    public class PipelineData {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "status")]
        public PipelineStatus Status { get; set; }
        [DataMember(Name = "ref")]
        public string Ref { get; set; }
        [DataMember(Name = "sha")]
        public Sha1 Sha1 { get; set; }
    }
}
