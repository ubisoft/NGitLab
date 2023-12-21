using NGitLab.Models;

namespace NGitLab.Impl;

internal sealed class ProjectVariableClient : VariableClient, IProjectVariableClient
{
    public ProjectVariableClient(API api, ProjectId projectId)
        : base(api, $"{Project.Url}/{projectId.ValueAsUriParameter()}")
    {
    }
}
