using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class UserClient : ClientBase, IUserClient
    {
        public UserClient(ClientContext context)
            : base(context)
        {
        }

        public Models.User this[int id]
        {
            get
            {
                using (Context.BeginOperationScope())
                {
                    return Server.Users.GetById(id)?.ToClientUser() ?? throw new GitLabNotFoundException();
                }
            }
        }

        public IEnumerable<Models.User> All
        {
            get
            {
                using (Context.BeginOperationScope())
                {
                    return Server.Users.Select(user => user.ToClientUser()).ToList();
                }
            }
        }

        public Session Current => Context.User?.ToClientSession();

        public ISshKeyClient CurrentUserSShKeys => throw new NotSupportedException();

        public Models.User Create(UserUpsert user)
        {
            using (Context.BeginOperationScope())
            {
                var u = new User(user.Username)
                {
                    Name = user.Name,
                    Email = user.Email,
                };

                Server.Users.Add(u);
                return u.ToClientUser();
            }
        }

        public UserToken CreateToken(UserTokenCreate tokenRequest)
        {
            throw new NotSupportedException();
        }

        public void Delete(int id)
        {
            using (Context.BeginOperationScope())
            {
                var user = Server.Users.GetById(id);

                if (user == null)
                {
                    throw new GitLabNotFoundException($"User '{id}' is not found. Cannot be deleted");
                }

                Server.Users.Remove(user);
            }
        }

        public void Activate(int id)
        {
            using (Context.BeginOperationScope())
            {
                var user = Server.Users.GetById(id);

                if (user == null)
                {
                    throw new GitLabNotFoundException($"User '{id}' is not found. Cannot be activated");
                }

                user.State = UserState.active;
            }
        }

        public void Deactivate(int id)
        {
            using (Context.BeginOperationScope())
            {
                var user = Server.Users.GetById(id);

                if (user == null)
                {
                    throw new GitLabNotFoundException($"User '{id}' is not found. Cannot be deactivated");
                }

                user.State = UserState.deactivated;
            }
        }

        public IEnumerable<Models.User> Get(string username)
        {
            using (Context.BeginOperationScope())
            {
                return Server.Users.SearchByUsername(username).Select(user => user.ToClientUser()).ToList();
            }
        }

        public IEnumerable<Models.User> Get(UserQuery query)
        {
            using (Context.BeginOperationScope())
            {
                return Server.Users.Get(query).Select(user => user.ToClientUser());
            }
        }

        public IEnumerable<Models.User> Search(string query)
        {
            using (Context.BeginOperationScope())
            {
                return Server.Users.SearchByUsername(query).Select(user => user.ToClientUser()).ToList();
            }
        }

        public ISshKeyClient SShKeys(int userId)
        {
            throw new NotImplementedException();
        }

        public Models.User Update(int id, UserUpsert userUpsert)
        {
            using (Context.BeginOperationScope())
            {
                var user = Server.Users.GetById(id);
                if (user != null)
                {
                    user.Name = userUpsert.Name;
                    user.Email = userUpsert.Email;

                    return user.ToClientUser();
                }

                throw new GitLabNotFoundException();
            }
        }

        public async Task<Models.User> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            return this[id];
        }

        public async Task<Session> GetCurrentUserAsync(CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            return Current;
        }

        public GitLabCollectionResponse<LastActivityDate> GetLastActivityDatesAsync(DateTimeOffset? from = null)
        {
            throw new NotImplementedException();
        }
    }
}
