using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class TagCreate {
        [DataMember(Name = "tag_name")]
        public string TagName { get; set; }

        [DataMember(Name = "ref")]
        public string Ref { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "release_description")]
        public string ReleaseDescription { get; set; }
    }
}