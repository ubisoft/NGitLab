using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class Tag {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "commit")]
        public CommitInfo Commit { get; set; }
    }
}