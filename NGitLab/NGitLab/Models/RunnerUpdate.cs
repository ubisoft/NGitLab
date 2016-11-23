using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class RunnerUpdate
    {
        [DataMember(Name = "active")]
        public bool Active;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "tag_list")]
        public string[] TagList;
    }
}