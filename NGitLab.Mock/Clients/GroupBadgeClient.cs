using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class GroupBadgeClient : ClientBase, IGroupBadgeClient
{
    private readonly long _groupId;

    public GroupBadgeClient(ClientContext context, GroupId groupId)
        : base(context)
    {
        _groupId = Server.AllGroups.FindGroup(groupId.ValueAsString()).Id;
    }

    public Models.Badge this[long id]
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                var group = GetGroup(_groupId, GroupPermission.View);
                var badge = group.Badges.GetById(id);
                if (badge == null)
                    throw new GitLabNotFoundException($"Badge with id '{id}' does not exist in group with id '{_groupId}'");

                return badge.ToBadgeModel();
            }
        }
    }

    public IEnumerable<Models.Badge> All
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                var group = GetGroup(_groupId, GroupPermission.View);
                return group.Badges.Select(badge => badge.ToBadgeModel()).ToList();
            }
        }
    }

    public Models.Badge Create(BadgeCreate badge)
    {
        EnsureUserIsAuthenticated();

        using (Context.BeginOperationScope())
        {
            var createdBadge = GetGroup(_groupId, GroupPermission.Edit).Badges.Add(badge.LinkUrl, badge.ImageUrl);
            return createdBadge.ToBadgeModel();
        }
    }

    public void Delete(long id)
    {
        EnsureUserIsAuthenticated();

        using (Context.BeginOperationScope())
        {
            var badgeToRemove = GetGroup(_groupId, GroupPermission.View).Badges.FirstOrDefault(b => b.Id == id);
            if (badgeToRemove == null)
            {
                throw new GitLabNotFoundException($"Badge with id '{id}' does not exist in group with id '{_groupId}'");
            }

            GetGroup(_groupId, GroupPermission.Edit).Badges.Remove(badgeToRemove);
        }
    }

    public Models.Badge Update(long id, BadgeUpdate badge)
    {
        using (Context.BeginOperationScope())
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
