using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class PersonInfo {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }
    }
}