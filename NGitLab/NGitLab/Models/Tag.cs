using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Tag
    {
        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "message")]
        public string Message;

        [DataMember(Name = "commit")]
        public CommitInfo Commit;

        [DataMember(Name = "release")]
        public RealeaseInfo Release;
    }
}