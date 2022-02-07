using System;
using System.Linq;

namespace NGitLab.Mock
{
    public sealed class BadgeCollection : Collection<Badge>
    {
        public BadgeCollection(GitLabObject parent)
            : base(parent)
        {
        }

        public Badge GetById(int id)
        {
            return this.FirstOrDefault(badge => badge.Id == id);
        }

        public override void Add(Badge item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (item.Id == default)
            {
                item.Id = Server.GetNewBadgeId();
            }
            else if (GetById(item.Id) != null)
            {
                throw new GitLabException("Badge already exists.");
            }

            base.Add(item);
        }

        public Badge Add(string linkUrl, string imageUrl)
        {
            var badge = new Badge
            {
                LinkUrl = linkUrl,
                ImageUrl = imageUrl,
            };

            Add(badge);

            return badge;
        }
    }
}
