using System.Collections.Generic;
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

        public IEnumerable<User> Filter(string query) => _api.Get().GetAll<User>(User.Url + $"?{query}");

        public User this[int id] => _api.Get().To<User>(User.Url + "/" + id.ToStringInvariant());

        public IEnumerable<User> Get(string username) => _api.Get().GetAll<User>(User.Url + "?username=" + username);

        public IEnumerable<User> Get(UserQuery query)
        {
            var url = User.Url;

            url = Utils.AddParameter(url, "active", query.IsActive);
            url = Utils.AddParameter(url, "blocked", query.IsBlocked);
            url = Utils.AddParameter(url, "external", query.IsExternal);
            url = Utils.AddParameter(url, "exclude_external", query.ExcludeExternal);
            url = Utils.AddParameter(url, "username", query.Username);
            url = Utils.AddParameter(url, "search", query.Search);
            url = Utils.AddParameter(url, "per_page", query.PerPage);
            url = Utils.AddParameter(url, "order_by", query.OrderBy);
            url = Utils.AddParameter(url, "sort", query.Sort);

            return _api.Get().GetAll<User>(url);
        }

        public User Create(UserUpsert user) => _api.Post().With(user).To<User>(User.Url);

        public UserToken CreateToken(UserTokenCreate tokenRequest) => _api.Post().With(tokenRequest).To<UserToken>(User.Url + "/" + tokenRequest.UserId.ToStringInvariant() + "/impersonation_tokens");

        public User Update(int id, UserUpsert user) => _api.Put().With(user).To<User>(User.Url + "/" + id.ToStringInvariant());

        public Session Current => _api.Get().To<Session>("/user");

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
    }
}
