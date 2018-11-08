using NGitLab.Models;

namespace NGitLab.Impl
{
    internal class GroupVariableClient : VariableClient, IGroupVariableClient
    {
        public GroupVariableClient(API api, int groupId)
            : base(api, Group.Url + $"/{groupId}")
        {
        }
    }
}
