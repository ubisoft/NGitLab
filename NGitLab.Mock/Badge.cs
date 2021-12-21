using NGitLab.Models;

namespace NGitLab.Mock
{
    public sealed class Badge : GitLabObject
    {
        public int Id { get; set; }

        public string LinkUrl { get; set; }

        public string ImageUrl { get; set; }

        public string RenderedLinkUrl { get; set; }

        public string RenderedImageUrl { get; set; }

        public BadgeKind Kind => Parent is Project ? BadgeKind.Project : BadgeKind.Group;

        public Models.Badge ToBadgeModel()
        {
            return new Models.Badge
            {
                Id = Id,
                ImageUrl = ImageUrl,
                Kind = Kind,
                LinkUrl = LinkUrl,
                RenderedImageUrl = RenderedImageUrl,
                RenderedLinkUrl = RenderedLinkUrl,
            };
        }
    }
}
