using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseLinkCreate
    {
        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "url")]
        public string Url;

        [DataMember(Name = "filepath")]
        public string Filepath;

        [DataMember(Name = "link_type")]
        public ReleaseLinkType LinkType;
    }
}
