using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface ISystemHookClient
{
    IEnumerable<SystemHook> All { get; }

    SystemHook this[long hookId] { get; }

    SystemHook Create(SystemHookUpsert hook);

    void Delete(long hookId);
}
