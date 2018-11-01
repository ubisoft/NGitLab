using NGitLab.Models;

namespace NGitLab.Impl
{
    internal class GroupBadgeClient : BadgeClient, IGroupBadgeClient
    {
        public GroupBadgeClient(API api, int projectId)
            : base(api, Group.Url + $"/{projectId}")
        {
        }
    }
}
