using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface ISystemHookClient
    {
        IEnumerable<SystemHook> All { get; }
        SystemHook this[int hookId] { get; }
        SystemHook Create(SystemHookUpsert hook);
        void Delete(int hookId);
    }
}
