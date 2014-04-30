using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Branch
    {
        public const string Url = "/repository/branches/";

        [DataMember(Name="name")]
        public string Name;

        [DataMember(Name="protected")]
        public bool Protected;
    }
}