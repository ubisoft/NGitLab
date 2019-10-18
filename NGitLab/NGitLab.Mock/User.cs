using System;
using System.Linq;

namespace NGitLab.Mock
{
    public sealed class User : GitLabObject
    {
        public User(string userName)
        {
            UserName = userName;
            Name = userName;
            Email = userName + "@example.com";
            State = UserState.active;
        }

        public int Id { get; set; }
        public string UserName { get; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public string WebUrl { get; set; }
        public string AvatarUrl { get; set; }
        public UserState State { get; set; }

        public Models.User ToClientUser()
        {
            var user = new Models.User();
            CopyTo(user);
            return user;
        }

        public Models.Session ToClientSession()
        {
            var user = new Models.Session();
            CopyTo(user);
            return user;
        }

        private void CopyTo<T>(T instance) where T : Models.User
        {
            instance.Id = Id;
            instance.Username = UserName;
            instance.Name = Name;
            instance.Email = Email;
        }

        public Group Namespace
        {
            get
            {
                var newGroup = Server.Groups.FirstOrDefault(group => string.Equals(@group.PathWithNameSpace, UserName, StringComparison.Ordinal));
                if (newGroup == null)
                {
                    newGroup = new Group(this);

                    Server.Groups.Add(newGroup);
                    newGroup.Permissions.Add(new Permission(this, Models.AccessLevel.Owner));
                }

                return newGroup;
            }
        }
    }
}
