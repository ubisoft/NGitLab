using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class MergeRequestComment {
        [DataMember(Name = "body")]
        public string Note { get; set; }
    }
}