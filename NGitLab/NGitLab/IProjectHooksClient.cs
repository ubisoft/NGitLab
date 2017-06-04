using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IProjectHooksClient
    {
        IEnumerable<ProjectHook> All();
        ProjectHook Get(int hookId);
        ProjectHook Create(ProjectHookUpsert hook);
        ProjectHook Update(int hookId, ProjectHookUpsert hook);
        void Delete(int hookId);
    }
}