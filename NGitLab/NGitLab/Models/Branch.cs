using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class Branch {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "commit")]
        public CommitInfo Commit { get; set; }

        [DataMember(Name = "protected")]
        public bool Protected { get; set; }
    }
}