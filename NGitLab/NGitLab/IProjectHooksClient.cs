using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab {
    public interface IProjectHooksClient {
        IEnumerable<ProjectHook> All();
        ProjectHook Get(int hookId);
        ProjectHook Create(ProjectHookInsert hook);
        ProjectHook Update(ProjectHookUpdate hook);
        void Delete(int hookId);
    }
}