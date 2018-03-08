using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Blob
    {
        [DataMember(Name = "size")]
        public int Size;

        [DataMember(Name = "encoding")]
        public string Encoding;

        [DataMember(Name = "content")]
        public string Content;

        [DataMember(Name = "sha")]
        public Sha1 Sha;
    }
}
