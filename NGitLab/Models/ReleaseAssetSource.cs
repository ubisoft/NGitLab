using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseAssetSource
    {
        [DataMember(Name = "format")]
        public string Format;

        [DataMember(Name = "url")]
        public string Url;
    }
}
