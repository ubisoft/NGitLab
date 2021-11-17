using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IGroupsClient
    {
        /// <summary>
        /// Get a list of projects accessible by the authenticated user.
        /// </summary>
        IEnumerable<Group> Accessible { get; }

        IEnumerable<Group> Search(string search);

        /// <summary>
        /// Get a list of GitLab groups.
        /// </summary>
        IEnumerable<Group> Get(GroupQuery query);

        Group this[int id] { get; }

        /// <summary>
        /// Returns the project with the provided full path in the form Namespace/Name.
        /// </summary>
        /// <remarks>https://github.com/gitlabhq/gitlabhq/blob/master/doc/api/groups.md#details-of-a-group</remarks>
        Group this[string fullPath] { get; }

        Group Create(GroupCreate group);

        IEnumerable<Project> SearchProjects(int groupId, string search);

        IEnumerable<Project> SearchProjects(SearchProjectQuery searchProjectQuery);

        void Delete(int id);

        Group Update(int id, GroupUpdate groupUpdate);

        void Restore(int id);
    }
}
