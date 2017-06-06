using System.Collections.Generic;
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

        public IEnumerable<ProjectHook> All()
        {
            return _api.Get().GetAll<ProjectHook>(_path);
        }

        public ProjectHook Get(int hookId)
        {
            return _api.Get().To<ProjectHook>(_path + "/" + hookId);
        }

        public ProjectHook Create(ProjectHookInsert hook)
        {
            return _api.Post().With(hook).To<ProjectHook>(_path);
        }

        public ProjectHook Update(ProjectHookUpdate hook)
        {
            return _api.Put().With(hook).To<ProjectHook>(_path + "/" + hook.HookId);
        }

        public void Delete(int hookId)
        {
            _api.Delete().To<ProjectHook>(_path + "/" + hookId);
        }
    }
}