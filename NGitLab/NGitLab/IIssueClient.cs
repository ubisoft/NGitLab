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
        /// Return a single issue for a project given project.
        /// 
        /// url like GET /projects/:id/issues/:issue_id
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="issueId"></param>
        /// <returns></returns>
        Issue Get(int projectId, int issueId);

        /// <summary>
        /// Add an issue witht he proposed title to the GitLab list for the selected proejct id.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="assigneeId"></param>
        /// <param name="milestoneId"></param>
        /// <param name="labels"></param>
        /// <returns>The issue if it was created.  Null if not.</returns>
        Issue Create(IssueCreate issueCreate);

        /// <summary>
        /// Edit and save an issue.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="issueId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="assigneeId"></param>
        /// <param name="milestoneId"></param>
        /// <param name="labels"></param>
        /// <param name="stateEvent"></param>
        /// <returns>The issue if it's updated.  Null if not.</returns>
        Issue Edit(IssueEdit issueEdit);
    }
}