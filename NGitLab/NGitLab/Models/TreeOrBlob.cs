using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class TreeOrBlob {
        [DataMember(Name = "id")]
        public Sha1 Id { get; set; }

        [DataMember(Name = "assets")]
        public string Name { get; set; }

        [DataMember(Name = "type")]
        public ObjectType Type { get; set; }

        [DataMember(Name = "mode")]
        public string Mode { get; set; }
    }
}