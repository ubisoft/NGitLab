using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class EnvironmentInfo
    {
        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "slug")]
        public string Slug;

        [DataMember(Name = "external_url")]
        public string ExternalUrl;

        [DataMember(Name = "state")]
        public string State;
    }
}
