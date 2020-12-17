using System.Collections.Generic;
using System.Linq;

namespace NGitLab.Mock.Clients
{
    internal sealed class GroupBadgeClient : ClientBase, IGroupBadgeClient
    {
        private readonly int _groupId;

        public GroupBadgeClient(ClientContext context, int groupId)
            : base(context)
        {
            _groupId = groupId;
        }

        public Models.Badge this[int id]
        {
            get
            {
                var group = GetGroup(_groupId, GroupPermission.View);
                var badge = group.Badges.GetById(id);
                if (badge == null)
                    throw new GitLabNotFoundException($"Badge with id '{id}' does not exist in group with id '{_groupId}'");

                return badge.ToBadgeModel();
            }
        }

        public IEnumerable<Models.Badge> All
        {
            get
            {
                var group = GetGroup(_groupId, GroupPermission.View);
                return group.Badges.Select(badge => badge.ToBadgeModel());
            }
        }

        public Models.Badge Create(Models.BadgeCreate badge)
        {
            EnsureUserIsAuthenticated();

            var createdBadge = GetGroup(_groupId, GroupPermission.Edit).Badges.Add(badge.LinkUrl, badge.ImageUrl);
            return createdBadge.ToBadgeModel();
        }

        public void Delete(int id)
        {
            EnsureUserIsAuthenticated();

            var badgeToRemove = GetGroup(_groupId, GroupPermission.View).Badges.FirstOrDefault(b => b.Id == id);
            if (badgeToRemove == null)
            {
                throw new GitLabNotFoundException($"Badge with id '{id}' does not exist in group with id '{_groupId}'");
            }

            GetGroup(_groupId, GroupPermission.Edit).Badges.Remove(badgeToRemove);
        }

        public Models.Badge Update(int id, Models.BadgeUpdate badge)
        {
            var badgeToUpdate = GetGroup(_groupId, GroupPermission.Edit).Badges.FirstOrDefault(b => b.Id == id);
            if (badgeToUpdate == null)
            {
                throw new GitLabNotFoundException($"Badge with id '{id}' does not exist in group with id '{_groupId}'");
            }

            badgeToUpdate.LinkUrl = badge.LinkUrl;
            badgeToUpdate.ImageUrl = badge.ImageUrl;
            return badgeToUpdate.ToBadgeModel();
        }
    }
}
