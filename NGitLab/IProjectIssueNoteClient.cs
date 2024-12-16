using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface IProjectIssueNoteClient
{
    /// <summary>
    /// Get a list of issue notes for the specified issue.
    /// </summary>
    IEnumerable<ProjectIssueNote> ForIssue(long issueId);

    /// <summary>
    /// Return a single issue note for a given.
    ///
    /// url like GET /projects/:id/issues/:issue_id
    ///
    /// </summary>
    /// <param name="issueId"></param>
    /// <param name="noteId"></param>
    /// <returns></returns>
    ProjectIssueNote Get(long issueId, long noteId);

    /// <summary>
    /// Add an project issue note with the specified body to the specified issue.
    /// </summary>
    /// <returns>The issue if it was created.  Null if not.</returns>
    ProjectIssueNote Create(ProjectIssueNoteCreate create);

    /// <summary>
    /// Edit and save an issue.
    /// </summary>
    /// <returns>The issue if it's updated.  Null if not.</returns>
    ProjectIssueNote Edit(ProjectIssueNoteEdit edit);
}
