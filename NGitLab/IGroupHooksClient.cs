using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface IGroupHooksClient
{
    IEnumerable<GroupHook> All { get; }

    GroupHook this[long hookId] { get; }

    GroupHook Create(GroupHookUpsert hook);

    GroupHook Update(long hookId, GroupHookUpsert hook);

    void Delete(long hookId);
}
