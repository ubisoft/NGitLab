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

        Group this[int id] { get; }

        Group Create(GroupCreate group);

        IEnumerable<Project> SearchProjects(int groupId, string search);

        void Delete(int id);

    }
}