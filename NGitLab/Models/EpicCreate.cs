using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class EpicCreate
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "labels")]
        public string Labels { get; set; }
    }
}
