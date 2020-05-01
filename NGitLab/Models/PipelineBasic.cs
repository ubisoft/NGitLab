using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class PipelineBasic
    {
        public const string Url = "/pipelines";

        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "status")]
        public JobStatus Status;

        [DataMember(Name = "ref")]
        public string Ref;

        [DataMember(Name = "sha")]
        public Sha1 Sha;
    }
}
