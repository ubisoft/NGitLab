using System;
using NGitLab.Models;

namespace NGitLab.Impl;

internal sealed class GroupVariableClient : VariableClient, IGroupVariableClient
{
    public GroupVariableClient(API api, GroupId groupId, string environmentScope = null)
        : base(api, string.IsNullOrWhiteSpace(environmentScope) ? $"{Group.Url}/{groupId.ValueAsUriParameter()}" : $"{Group.Url}/{groupId.ValueAsUriParameter()}?filter[environment_scope]={Uri.EscapeDataString(environmentScope!)}")
    {
    }
}
