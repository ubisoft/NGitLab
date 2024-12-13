using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

public class UserClient : IUserClient
{
    private readonly API _api;

    public UserClient(API api)
    {
        _api = api;
    }

    public IEnumerable<User> All => _api.Get().GetAll<User>(User.Url);

    public IEnumerable<User> Search(string query) => _api.Get().GetAll<User>(User.Url + $"?search={query}");

    public User this[long id] => _api.Get().To<User>(User.Url + "/" + id.ToStringInvariant());

    public Task<User> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return _api.Get().ToAsync<User>(User.Url + "/" + id.ToStringInvariant(), cancellationToken);
    }

    public async Task<User> GetByUserNameAsync(string username, CancellationToken cancellationToken = default)
    {
        // This query returns 0 or 1 user.
        await foreach (var u in _api.Get().GetAllAsync<User>(User.Url + "?username=" + username).WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            return u;
        }

        throw new GitLabException("User not found.")
        {
            StatusCode = System.Net.HttpStatusCode.NotFound,
            ErrorMessage = "User not found.",
        };
    }

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
        url = Utils.AddParameter(url, "extern_uid", query.ExternalUid);
        url = Utils.AddParameter(url, "provider", query.Provider);
        url = Utils.AddParameter(url, "without_projects", query.WithoutProjects);
        url = Utils.AddParameter(url, "created_before", query.CreatedBefore);
        url = Utils.AddParameter(url, "created_after", query.CreatedAfter);
        url = Utils.AddParameter(url, "with_custom_attributes", query.WithCustomAttributes);
        url = Utils.AddParameter(url, "two_factor", query.TwoFactor);
        url = Utils.AddParameter(url, "admins", query.IsAdmin);

        return _api.Get().GetAll<User>(url);
    }

    public User Create(UserUpsert user) => _api.Post().With(user).To<User>(User.Url);

    public Task<User> CreateAsync(UserUpsert user, CancellationToken cancellationToken = default) => _api.Post().With(user).ToAsync<User>(User.Url, cancellationToken);

    public UserToken CreateToken(UserTokenCreate tokenRequest) =>
        _api.Post().With(tokenRequest).To<UserToken>($"{User.Url}/{tokenRequest.UserId.ToStringInvariant()}/impersonation_tokens");

    public Task<UserToken> CreateTokenAsync(UserTokenCreate tokenRequest, CancellationToken cancellationToken = default) =>
        _api.Post().With(tokenRequest).ToAsync<UserToken>($"{User.Url}/{tokenRequest.UserId.ToStringInvariant()}/impersonation_tokens", cancellationToken);

    public User Update(long id, UserUpsert user) => _api.Put().With(user).To<User>(User.Url + "/" + id.ToStringInvariant());

    public Session Current => _api.Get().To<Session>("/user");

    public Task<Session> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        return _api.Get().ToAsync<Session>("/user", cancellationToken);
    }

    public ISshKeyClient CurrentUserSShKeys => new SshKeyClient(_api, userId: null);

    public ISshKeyClient SShKeys(long userId) => new SshKeyClient(_api, userId);

    public void Delete(long userId)
    {
        _api.Delete().Execute(User.Url + "/" + userId.ToStringInvariant());
    }

    public void Activate(long userId)
    {
        _api.Post().Execute($"{User.Url}/{userId.ToStringInvariant()}/activate");
    }

    public void Deactivate(long userId)
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
