using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface IProjectHooksClient
{
    IEnumerable<ProjectHook> All { get; }

    ProjectHook this[long hookId] { get; }

    ProjectHook Create(ProjectHookUpsert hook);

    ProjectHook Update(long hookId, ProjectHookUpsert hook);

    void Delete(long hookId);
}
