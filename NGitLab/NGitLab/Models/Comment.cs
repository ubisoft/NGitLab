using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class Comment {
        [DataMember(Name = "note")]
        public string Note { get; set; }

        [DataMember(Name = "author")]
        public User Author { get; set; }
    }
}