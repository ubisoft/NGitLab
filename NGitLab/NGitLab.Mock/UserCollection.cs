using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NGitLab.Mock
{
    public sealed class UserCollection : Collection<User>
    {
        public UserCollection(GitLabObject container)
            : base(container)
        {
        }

        public User GetById(string id)
        {
            if (int.TryParse(id, NumberStyles.None, CultureInfo.InvariantCulture, out var value))
                return GetById(value);

            return null;
        }

        public User GetById(int id) => this.FirstOrDefault(user => user.Id == id);

        public User AddNew()
        {
            var userName = "user" + Guid.NewGuid().ToString("N");
            return Add(userName);
        }

        public User Add(string userName)
        {
            var user = new User(userName);
            Add(user);
            return user;
        }

        public override void Add(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            if (user.Id == default)
            {
                user.Id = GetNewId();
            }
            else if (GetById(user.Id) != null)
            {
                throw new GitLabException("User already exists");
            }

            Server.Groups.Add(new Group(user));

            base.Add(user);
        }

        private int GetNewId()
        {
            return this.Select(user => user.Id).DefaultIfEmpty().Max() + 1;
        }

        internal IEnumerable<User> SearchByUsername(string username)
        {
            return this.Where(user => string.Equals(user.UserName, username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
