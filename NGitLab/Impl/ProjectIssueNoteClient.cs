using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl;

public class ProjectIssueNoteClient : IProjectIssueNoteClient
{
    private const string IssuesNoteUrl = "/projects/{0}/issues/{1}/notes";
    private const string SingleNoteIssueUrl = "/projects/{0}/issues/{1}/notes/{2}";

    private readonly API _api;
    private readonly string _projectId;

    public ProjectIssueNoteClient(API api, ProjectId projectId)
    {
        _api = api;
        _projectId = projectId.ValueAsUriParameter();
    }

    public IEnumerable<ProjectIssueNote> ForIssue(long issueId)
    {
        return _api.Get().GetAll<ProjectIssueNote>(string.Format(IssuesNoteUrl, _projectId, issueId));
    }

    public ProjectIssueNote Get(long issueId, long noteId)
    {
        return _api.Get().To<ProjectIssueNote>(string.Format(SingleNoteIssueUrl, _projectId, issueId, noteId));
    }

    public ProjectIssueNote Create(ProjectIssueNoteCreate create)
    {
        return _api.Post().With(create).To<ProjectIssueNote>(string.Format(IssuesNoteUrl, _projectId, create.IssueId));
    }

    public ProjectIssueNote Edit(ProjectIssueNoteEdit edit)
    {
        return _api.Put().With(edit).To<ProjectIssueNote>(string.Format(SingleNoteIssueUrl, _projectId, edit.IssueId, edit.NoteId));
    }
}
