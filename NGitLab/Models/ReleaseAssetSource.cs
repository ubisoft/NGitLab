using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseAssetSource
    {
        [DataMember(Name = "format")]
        public string Format { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}
