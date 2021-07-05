using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseAssetsInfo
    {
        [DataMember(Name = "count")]
        public int? Count;

        [DataMember(Name = "sources")]
        public ReleaseAssetSource[] Sources;

        [DataMember(Name = "links")]
        public ReleaseLink[] Links;
    }
}
