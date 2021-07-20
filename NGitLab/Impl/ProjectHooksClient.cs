using System.Collections.Generic;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class ProjectHooksClient : IProjectHooksClient
    {
        private readonly API _api;
        private readonly string _path;

        public ProjectHooksClient(API api, string projectPath)
        {
            _api = api;
            _path = projectPath + "/hooks";
        }

        public IEnumerable<ProjectHook> All => _api.Get().GetAll<ProjectHook>(_path);

        public ProjectHook this[int hookId] => _api.Get().To<ProjectHook>(_path + "/" + hookId.ToStringInvariant());

        public ProjectHook Create(ProjectHookUpsert hook) => _api.Post().With(hook).To<ProjectHook>(_path);

        public ProjectHook Update(int hookId, ProjectHookUpsert hook) => _api.Put().With(hook).To<ProjectHook>(_path + "/" + hookId.ToStringInvariant());

        public void Delete(int hookId)
        {
            _api.Delete().Execute(_path + "/" + hookId.ToStringInvariant());
        }
    }
}
