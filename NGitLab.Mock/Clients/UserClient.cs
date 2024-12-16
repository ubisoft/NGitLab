using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class UserClient : ClientBase, IUserClient
{
    public UserClient(ClientContext context)
        : base(context)
    {
    }

    public Models.User this[long id]
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

    public Session Current
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                return Context.User?.ToClientSession();
            }
        }
    }

    public ISshKeyClient CurrentUserSShKeys => throw new NotSupportedException();

    public Models.User Create(UserUpsert user)
    {
        using (Context.BeginOperationScope())
        {
            var u = new User(user.Username)
            {
                Name = user.Name,
                Email = user.Email,
                State = UserState.active,
                IsAdmin = user.IsAdmin ?? false,
            };

            Server.Users.Add(u);
            return u.ToClientUser();
        }
    }

    public async Task<Models.User> CreateAsync(UserUpsert user, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return Create(user);
    }

    public UserToken CreateToken(UserTokenCreate tokenRequest)
    {
        throw new NotSupportedException();
    }

    public async Task<UserToken> CreateTokenAsync(UserTokenCreate tokenRequest, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return CreateToken(tokenRequest);
    }

    public void Delete(long id)
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

    public void Activate(long id)
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

    public void Deactivate(long id)
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
            return Server.Users
                .Where(user => string.Equals(user.Email, query, StringComparison.OrdinalIgnoreCase) || string.Equals(user.UserName, query, StringComparison.OrdinalIgnoreCase))
                .Select(user => user.ToClientUser()).ToList();
        }
    }

    public ISshKeyClient SShKeys(long userId)
    {
        throw new NotImplementedException();
    }

    public Models.User Update(long id, UserUpsert userUpsert)
    {
        using (Context.BeginOperationScope())
        {
            var user = Server.Users.GetById(id);
            if (user != null)
            {
                // user.UserName = userUpsert.Username ?? user.UserName; // TODO
                user.Name = userUpsert.Name ?? user.Name;
                user.Email = userUpsert.Email ?? user.Email;
                user.IsAdmin = userUpsert.IsAdmin ?? user.IsAdmin;

                return user.ToClientUser();
            }

            throw new GitLabNotFoundException();
        }
    }

    public async Task<Models.User> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return this[id];
    }

    public async Task<Models.User> GetByUserNameAsync(string username, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return Get(username).SingleOrDefault()
            ?? throw new GitLabException("User not found.")
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "User not found.",
            };
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
