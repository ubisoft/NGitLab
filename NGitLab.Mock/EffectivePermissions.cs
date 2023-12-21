using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock;

public sealed class EffectivePermissions
{
    internal EffectivePermissions(IReadOnlyCollection<EffectiveUserPermission> permissions)
    {
        Permissions = permissions;
    }

    public IReadOnlyCollection<EffectiveUserPermission> Permissions { get; }

    public AccessLevel? GetAccessLevel(User user)
    {
        return GetEffectivePermission(user)?.AccessLevel;
    }

    public EffectiveUserPermission GetEffectivePermission(User user)
    {
        return Permissions.FirstOrDefault(p => p.User == user);
    }
}
