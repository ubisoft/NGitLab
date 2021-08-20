using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestChange
    {
        [DataMember(Name = "changes")]
        public Change[] Changes { get; set; }
    }
}
