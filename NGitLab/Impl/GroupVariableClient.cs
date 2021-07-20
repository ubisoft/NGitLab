using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl
{
    internal sealed class GroupVariableClient : VariableClient, IGroupVariableClient
    {
        public GroupVariableClient(API api, int groupId)
            : base(api, Group.Url + $"/{groupId.ToStringInvariant()}")
        {
        }
    }
}
