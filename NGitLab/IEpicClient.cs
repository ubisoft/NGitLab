using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface IEpicClient
{
    /// <summary>
    /// Return all group's epics
    /// </summary>
    /// <param name="groupId">Group ID</param>
    /// <param name="query">Filtering and ordering query</param>
    /// <returns></returns>
    IEnumerable<Epic> Get(long groupId, EpicQuery query);

    /// <summary>
    /// Return a group epic
    /// </summary>
    /// <param name="groupId">Group ID</param>
    /// <param name="epicId">Epic ID</param>
    /// <returns></returns>
    Epic Get(long groupId, long epicId);

    /// <summary>
    /// Return all issues that are assigned to an epic
    /// </summary>
    /// <param name="groupId">Group ID</param>
    /// <param name="epicId">Epic ID</param>
    GitLabCollectionResponse<Issue> GetIssuesAsync(long groupId, long epicId);

    /// <summary>
    /// Create an epic
    /// </summary>
    /// <param name="groupId">Group ID</param>
    /// <param name="epic">Epic to create</param>
    /// <returns></returns>
    Epic Create(long groupId, EpicCreate epic);

    /// <summary>
    /// Update an epic
    /// </summary>
    /// <param name="groupId">Group ID</param>
    /// <param name="epicEdit">New properties values</param>
    /// <returns></returns>
    Epic Edit(long groupId, EpicEdit epicEdit);
}
