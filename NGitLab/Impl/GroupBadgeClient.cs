using NGitLab.Models;

namespace NGitLab.Impl;

internal sealed class GroupBadgeClient : BadgeClient, IGroupBadgeClient
{
    public GroupBadgeClient(API api, GroupId groupId)
        : base(api, $"{Group.Url}/{groupId.ValueAsUriParameter()}")
    {
    }
}
