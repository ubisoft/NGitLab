using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class CommitStatus
    {
        [DataMember(Name = "id")]
        public int ProjectId;

        [DataMember(Name = "sha")]
        public string CommitSha;

        [DataMember(Name = "ref")]
        public string Ref;

        [DataMember(Name = "status")]
        public string Status;

        [DataMember(Name = "name")]
        public string Name;
    }
}
