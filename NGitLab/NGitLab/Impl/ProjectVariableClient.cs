using NGitLab.Models;

namespace NGitLab.Impl
{
    internal class ProjectVariableClient : VariableClient, IProjectVariableClient
    {
        public ProjectVariableClient(API api, int projectId)
            : base(api, Project.Url + $"/{projectId}")
        {
        }
    }
}
