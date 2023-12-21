using NGitLab.Models;

namespace NGitLab.Impl;

internal sealed class GroupVariableClient : VariableClient, IGroupVariableClient
{
    public GroupVariableClient(API api, GroupId groupId)
        : base(api, $"{Group.Url}/{groupId.ValueAsUriParameter()}")
    {
    }
}
