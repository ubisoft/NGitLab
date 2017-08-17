using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Namespace
    {
        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "path")]
        public string Path;

        [DataMember(Name = "kind")]
        public string Kind;

        [DataMember(Name = "full_path")]
        public string FullPath;

        public enum Type
        {
            Group,
            User
        }

        public Type GetKind()
        {
            return (Type) Enum.Parse(typeof(Type), Kind, true);
        }
    }
}
