using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl
{
    internal sealed class ProjectVariableClient : VariableClient, IProjectVariableClient
    {
        public ProjectVariableClient(API api, int projectId)
            : base(api, Project.Url + $"/{projectId.ToStringInvariant()}")
        {
        }
    }
}
