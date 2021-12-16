using System.Diagnostics;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    [DebuggerDisplay("{Path} ({Type})")]
    public class Tree
    {
        [DataMember(Name = "id")]
        public Sha1 Id;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "type")]
        public ObjectType Type;

        [DataMember(Name = "mode")]
        public string Mode;

        [DataMember(Name = "path")]
        public string Path;
    }
}
