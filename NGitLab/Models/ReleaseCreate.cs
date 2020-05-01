using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseCreate
    {
        [DataMember(Name = "description")]
        public string Description;
    }
}
