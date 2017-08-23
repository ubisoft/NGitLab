using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab {
    public interface IIssueClient {
        /// <summary>
        ///     Get a list of all project issues
        /// </summary>
        IEnumerable<Issue> Owned();

        /// <summary>
        ///     Get a list of issues for the specified project.
        /// </summary>
        IEnumerable<Issue> ForProject(int projectId);
    }
}