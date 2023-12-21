using NGitLab.Models;

namespace NGitLab.Mock;

public sealed class EffectiveUserPermission
{
    internal EffectiveUserPermission(User user, AccessLevel accessLevel)
    {
        User = user;
        AccessLevel = accessLevel;
    }

    public User User { get; }

    public AccessLevel AccessLevel { get; }

    public Membership ToMembershipClient()
    {
        return new Membership
        {
            Id = User.Id,
            AvatarURL = User.AvatarUrl,
            UserName = User.UserName,
            Name = User.Name,
            AccessLevel = (int)AccessLevel,
            State = User.State.ToString().ToLowerInvariant(),
        };
    }
}
