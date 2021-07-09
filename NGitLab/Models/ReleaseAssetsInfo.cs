using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseAssetsInfo
    {
        [DataMember(Name = "count")]
        public int? Count { get; set; }

        [DataMember(Name = "sources")]
        public ReleaseAssetSource[] Sources { get; set; }

        [DataMember(Name = "links")]
        public ReleaseLink[] Links { get; set; }
    }
}
