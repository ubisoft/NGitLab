using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab {
    public interface IProjectClient {
        /// <summary>
        ///     Get a list of projects accessible by the authenticated user.
        /// </summary>
        IEnumerable<Project> Accessible();

        /// <summary>
        ///     Get a list of projects owned by the authenticated user.
        /// </summary>
        IEnumerable<Project> Owned();

        /// <summary>
        ///     Gets a list of starred projects.
        /// </summary>
        IEnumerable<Project> Starred();

        Project Get(int id);

        Project Create(ProjectCreate project);

        bool Delete(int id);

        Project Star(int id);
    }
}