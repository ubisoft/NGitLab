using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl {
    public class ProjectHooksClient : IProjectHooksClient {
        readonly Api api;
        readonly string path;

        public ProjectHooksClient(Api api, string projectPath) {
            this.api = api;
            path = projectPath + "/hooks";
        }

        public IEnumerable<ProjectHook> All => api.Get().GetAll<ProjectHook>(path);
        

        public ProjectHook Get(int hookId) {
            return api.Get().To<ProjectHook>(path + "/" + hookId);
        }

        public ProjectHook Create(ProjectHookInsert hook) {
            return api.Post().With(hook).To<ProjectHook>(path);
        }

        public ProjectHook Update(ProjectHookUpdate hook) {
            return api.Put().With(hook).To<ProjectHook>(path + "/" + hook.HookId);
        }

        public void Delete(int hookId) {
            api.Delete().To<ProjectHook>(path + "/" + hookId);
        }
    }
}