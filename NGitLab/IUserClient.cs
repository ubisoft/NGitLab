using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab
{
    public interface IUserClient
    {
        IEnumerable<User> All { get; }

        IEnumerable<User> Search(string query);

        User this[int id] { get; }

        Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        User Create(UserUpsert user);

        User Update(int id, UserUpsert user);

        /// <summary>
        /// Request a token for user impersonation.
        /// Admin account/token is required for impersonation
        /// </summary>
        /// <param name="tokenRequest">info required to create the token</param>
        /// <returns></returns>
        UserToken CreateToken(UserTokenCreate tokenRequest);

        void Delete(int id);

        void Activate(int id);

        void Deactivate(int id);

        Session Current { get; }

        Task<Session> GetCurrentUserAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Allows to manipulate the ssh keys of the currently logged user.
        /// Does not need admin rights.
        /// </summary>
        ISshKeyClient CurrentUserSShKeys { get; }

        /// <summary>
        /// Allows to manipulate the ssh keys of a given user, needs GitLab admin rights.
        /// </summary>
        ISshKeyClient SShKeys(int userId);

        /// <summary>
        /// Get users that match the username
        /// </summary>
        IEnumerable<User> Get(string username);

        /// <summary>
        /// Get a list of users that match the query.
        /// </summary>
        IEnumerable<User> Get(UserQuery query);

        /// <summary>
        /// Gets the last activity date for all users, sorted from oldest to newest
        /// (<seealso href="https://docs.gitlab.com/ee/api/users.html#get-user-activities-admin-only">GitLab documentation</seealso>).
        /// </summary>
        /// <param name="from">Date from which activities will be considered. If unspecified, will look back over the last 6 months.</param>
        /// <remarks>
        /// The activities that update the timestamp are:
        /// <list type="bullet">
        /// <item><description>Git HTTP/SSH activities (such as clone, push)</description></item>
        /// <item><description>User logging in to GitLab</description></item>
        /// <item><description>User visiting pages related to dashboards, projects, issues, and merge requests (introduced in GitLab 11.8)</description></item>
        /// <item><description>User using the API</description></item>
        /// <item><description>User using the GraphQL API</description></item>
        /// </list>
        /// </remarks>
        GitLabCollectionResponse<LastActivityDate> GetLastActivityDatesAsync(DateTimeOffset? from = null);
    }
}
