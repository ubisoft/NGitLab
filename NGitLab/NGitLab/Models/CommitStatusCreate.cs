using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class CommitStatusCreate
    {
        [DataMember(Name = "id")]
        public int ProjectId;

        [DataMember(Name = "sha")]
        public string CommitSha;

        [DataMember(Name = "state")]
        public string State;

        [DataMember(Name = "status")]
        public string Status;

        [DataMember(Name = "ref")]
        public string Ref;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "target_url")]
        public string TargetUrl;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "coverage")]
        public int Coverage;
    }
}
