using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class UserClient : IUserClient
    {
        private readonly API _api;

        public UserClient(API api)
        {
            _api = api;
        }

        public IEnumerable<User> All => _api.Get().GetAll<User>(User.Url);

        public IEnumerable<User> Search(string query) => _api.Get().GetAll<User>(User.Url + $"?search={query}");

        public User this[int id] => _api.Get().To<User>(User.Url + "/" + id.ToStringInvariant());

        public Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _api.Get().ToAsync<User>(User.Url + "/" + id.ToStringInvariant(), cancellationToken);
        }

        public IEnumerable<User> Get(string username) => _api.Get().GetAll<User>(User.Url + "?username=" + username);

        public IEnumerable<User> Get(UserQuery query)
        {
            var url = QueryStringHelper.BuildAndAppendQueryString(User.Url, query);
            return _api.Get().GetAll<User>(url);
        }

        public User Create(UserUpsert user) => _api.Post().With(user).To<User>(User.Url);

        public UserToken CreateToken(UserTokenCreate tokenRequest) => _api.Post().With(tokenRequest).To<UserToken>(User.Url + "/" + tokenRequest.UserId.ToStringInvariant() + "/impersonation_tokens");

        public User Update(int id, UserUpsert user) => _api.Put().With(user).To<User>(User.Url + "/" + id.ToStringInvariant());

        public Session Current => _api.Get().To<Session>("/user");

        public Task<Session> GetCurrentUserAsync(CancellationToken cancellationToken = default)
        {
            return _api.Get().ToAsync<Session>("/user", cancellationToken);
        }

        public ISshKeyClient CurrentUserSShKeys => new SshKeyClient(_api, userId: null);

        public ISshKeyClient SShKeys(int userId) => new SshKeyClient(_api, userId);

        public void Delete(int userId)
        {
            _api.Delete().Execute(User.Url + "/" + userId.ToStringInvariant());
        }

        public void Activate(int userId)
        {
            _api.Post().Execute($"{User.Url}/{userId.ToStringInvariant()}/activate");
        }

        public void Deactivate(int userId)
        {
            _api.Post().Execute($"{User.Url}/{userId.ToStringInvariant()}/deactivate");
        }

        public GitLabCollectionResponse<LastActivityDate> GetLastActivityDatesAsync(DateTimeOffset? from = null)
        {
            var url = "/user/activities";
            if (from is not null)
                url = Utils.AddParameter(url, "from", from.Value.ToString("yyyy-MM-dd"));
            return _api.Get().GetAllAsync<LastActivityDate>(url);
        }
    }
}
