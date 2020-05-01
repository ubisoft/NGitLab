using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Badge
    {
        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "link_url")]
        public string LinkUrl;

        [DataMember(Name = "image_url")]
        public string ImageUrl;

        [DataMember(Name = "rendered_link_url")]
        public string RenderedLinkUrl;

        [DataMember(Name = "rendered_image_url")]
        public string RenderedImageUrl;

        [DataMember(Name = "kind")]
        public BadgeKind Kind;
    }
}
