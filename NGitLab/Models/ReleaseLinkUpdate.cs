using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseLinkUpdate
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "filepath")]
        public string Filepath { get; set; }

        [DataMember(Name = "link_type")]
        public ReleaseLinkType LinkType { get; set; }
    }
}
