using System.Runtime.Serialization;

namespace NGitLab.Models {
    public enum PipelineStatus {
        undefined,
        pending,
        success,
        created,
        failed,
        aborted,
        running,
        canceled
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
