using System;
using System.Net;
using NGitLab.Models;

namespace NGitLab.Impl;

public class CommitClient : ICommitClient
{
    private readonly API _api;
    private readonly string _repoPath;

    public CommitClient(API api, ProjectId projectId)
    {
        _api = api;
        var projectPath = $"{Project.Url}/{projectId.ValueAsUriParameter()}";
        _repoPath = $"{projectPath}/repository";
    }

    public Commit GetCommit(string @ref)
    {
        return _api.Get().To<Commit>(_repoPath + $"/commits/{@ref}");
    }

    public Commit CherryPick(CommitCherryPick cherryPick)
    {
        return _api.Post().With(cherryPick).To<Commit>($"{_repoPath}/commits/{cherryPick.Sha}/cherry_pick");
    }

    public JobStatus GetJobStatus(string branchName)
    {
        var encodedBranch = WebUtility.UrlEncode(branchName);

        var latestCommit = _api.Get().To<Commit>(_repoPath + $"/commits/{encodedBranch}?per_page=1");
        if (latestCommit == null)
        {
            return JobStatus.Unknown;
        }

        if (string.IsNullOrEmpty(latestCommit.Status))
        {
            return JobStatus.NoBuild;
        }

        if (!Enum.TryParse(latestCommit.Status, ignoreCase: true, result: out JobStatus result))
        {
            throw new NotSupportedException($"Status {latestCommit.Status} is unrecognised");
        }

        return result;
    }

    public Commit Create(CommitCreate commit) => _api.Post().With(commit).To<Commit>(_repoPath + "/commits");

    public GitLabCollectionResponse<MergeRequest> GetRelatedMergeRequestsAsync(RelatedMergeRequestsQuery query)
    {
        return _api.Get().GetAllAsync<MergeRequest>(_repoPath + $"/commits/{query.Sha}/merge_requests");
    }

    public Commit Revert(CommitRevert revert)
    {
        return _api.Post().With(revert).To<Commit>($"{_repoPath}/commits/{revert.Sha}/revert");
    }
}
