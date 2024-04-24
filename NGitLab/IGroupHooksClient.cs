using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface IGroupHooksClient
{
    IEnumerable<GroupHook> All { get; }

    GroupHook this[int hookId] { get; }

    GroupHook Create(GroupHookUpsert hook);

    GroupHook Update(int hookId, GroupHookUpsert hook);

    void Delete(int hookId);
}
