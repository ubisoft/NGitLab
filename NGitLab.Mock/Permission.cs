using System;
using NGitLab.Models;

namespace NGitLab.Mock
{
    public sealed class Permission : GitLabObject
    {
        public Permission(User user, AccessLevel accessLevel)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            AccessLevel = accessLevel;
        }

        public Permission(Group group, AccessLevel accessLevel)
        {
            Group = group ?? throw new ArgumentNullException(nameof(group));
            AccessLevel = accessLevel;
        }

        public User User { get; }

        public Group Group { get; }

        public AccessLevel AccessLevel { get; }

        internal Membership ToUserMembership()
        {
            if (User is null)
                return null;

            return new Membership
            {
                Id = User.Id,
                AvatarURL = User.AvatarUrl,
                CreatedAt = User.CreatedAt,
                Name = User.Name,
                State = User.State.ToString().ToLowerInvariant(),
                UserName = User.UserName,
                AccessLevel = (int)AccessLevel,
            };
        }
    }
}
