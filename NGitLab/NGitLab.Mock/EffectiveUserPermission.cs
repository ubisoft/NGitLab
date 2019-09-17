using NGitLab.Models;

namespace NGitLab.Mock
{
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
                UserName = User.UserName,
                Name = User.Name,
                AccessLevel = (int)AccessLevel,
            };
        }
    }
}
