using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class MergeRequestComment {
        [DataMember(Name = "note")]
        public string Note { get; set; }
    }
}