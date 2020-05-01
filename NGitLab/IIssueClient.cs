using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IIssueClient
    {
        /// <summary>
        /// Get a list of all project issues
        /// </summary>
        IEnumerable<Issue> Owned { get; }

        /// <summary>
        /// Get a list of issues for the specified project.
        /// </summary>
        IEnumerable<Issue> ForProject(int projectId);

        /// <summary>
        ///     <para>Return a single issue for a project given project.</para>
        ///     <para>url like GET /projects/:id/issues/:issue_id</para>
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="issueId"></param>
        /// <returns>The issue that corresponds to the project id and issue id</returns>
        Issue Get(int projectId, int issueId);

        /// <summary>
        /// Return issues for a given query.
        ///
        /// url like GET /issues?<parameter_name>=<value>
        ///
        /// </summary>
        /// <param name="query"></param>
        /// <returns>the query's related issues</returns>
        IEnumerable<Issue> Get(IssueQuery query);

        /// <summary>
        /// Return project issues for a given query.
        ///
        /// url like GET /projects/:id/issues?<parameter_name>=<value>
        ///
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="query"></param>
        /// <returns>the query's related issues</returns>
        IEnumerable<Issue> Get(int projectId, IssueQuery query);

        /// <summary>
        /// Add an issue witht he proposed title to the GitLab list for the selected proejct id.
        /// </summary>
        /// <param name="issueCreate"></param>
        /// <returns>The issue if it was created.  Null if not.</returns>
        Issue Create(IssueCreate issueCreate);

        /// <summary>
        /// Edit and save an issue.
        /// </summary>
        /// <param name="issueEdit"></param>
        /// <returns>The issue if it's updated.  Null if not.</returns>
        Issue Edit(IssueEdit issueEdit);
    }
}
