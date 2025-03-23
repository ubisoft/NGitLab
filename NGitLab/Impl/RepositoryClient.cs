﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

public class RepositoryClient : IRepositoryClient
{
    private readonly API _api;
    private readonly string _repoPath;
    private readonly string _projectPath;

    public RepositoryClient(API api, ProjectId projectId)
    {
        _api = api;
        _projectPath = $"{Project.Url}/{projectId.ValueAsUriParameter()}";
        _repoPath = $"{_projectPath}/repository";
    }

    public ITagClient Tags => new TagClient(_api, _repoPath);

    public IContributorClient Contributors => new ContributorClient(_api, _repoPath);

    public IEnumerable<Tree> Tree => _api.Get().GetAll<Tree>(_repoPath + "/tree");

    public IEnumerable<Tree> GetTree(string path) => GetTree(new RepositoryGetTreeOptions { Path = path });

    public IEnumerable<Tree> GetTree(string path, string @ref, bool recursive) => GetTree(new RepositoryGetTreeOptions { Path = path, Ref = @ref, Recursive = recursive });

    public IEnumerable<Tree> GetTree(RepositoryGetTreeOptions options)
    {
        var url = BuildGetTreeUrl(options);
        return _api.Get().GetAll<Tree>(url);
    }

    public GitLabCollectionResponse<Tree> GetTreeAsync(RepositoryGetTreeOptions options)
    {
        var url = BuildGetTreeUrl(options);
        return _api.Get().GetAllAsync<Tree>(url);
    }

    public void GetRawBlob(string sha, Action<Stream> parser)
    {
        _api.Get().Stream(_repoPath + "/blobs/" + sha + "/raw", parser);
    }

    public void GetArchive(Action<Stream> parser)
    {
        _api.Get().Stream(_repoPath + "/archive", parser);
    }

    public IEnumerable<Commit> Commits => _api.Get().GetAll<Commit>(_repoPath + $"/commits?per_page={GetCommitsRequest.DefaultPerPage}");

    /// <summary>
    /// Gets all the commits of the specified branch/tag.
    /// </summary>
    public IEnumerable<Commit> GetCommits(string refName, int maxResults = 0)
    {
        return GetCommits(new GetCommitsRequest { MaxResults = maxResults, RefName = refName });
    }

    /// <summary>
    /// Gets all the commits of the specified branch/tag.
    /// </summary>
    public IEnumerable<Commit> GetCommits(GetCommitsRequest request)
    {
        var lst = new List<string>();
        if (!string.IsNullOrWhiteSpace(request.RefName))
        {
            lst.Add($"ref_name={Uri.EscapeDataString(request.RefName)}");
        }

        if (!string.IsNullOrWhiteSpace(request.Path))
        {
            lst.Add($"path={Uri.EscapeDataString(request.Path)}");
        }

        if (request.FirstParent != null)
        {
            lst.Add($"first_parent={Uri.EscapeDataString(request.FirstParent.ToString())}");
        }

        if (request.Since.HasValue)
        {
            lst.Add($"since={Uri.EscapeDataString(request.Since.Value.ToString("s", CultureInfo.InvariantCulture))}");
        }

        if (request.Until.HasValue)
        {
            lst.Add($"until={Uri.EscapeDataString(request.Until.Value.ToString("s", CultureInfo.InvariantCulture))}");
        }

        var perPage = request.MaxResults > 0 ? Math.Min(request.MaxResults, request.PerPage) : request.PerPage;
        lst.Add($"per_page={perPage.ToStringInvariant()}");

        var path = _repoPath + "/commits" + (lst.Count == 0 ? string.Empty : "?" + string.Join("&", lst));
        var allCommits = _api.Get().GetAll<Commit>(path);
        if (request.MaxResults <= 0)
            return allCommits;

        return allCommits.Take(request.MaxResults);
    }

    public CompareResults Compare(CompareQuery query)
    {
        return _api.Get().To<CompareResults>(BuildCompareUrl(query));
    }

    public Task<CompareResults> CompareAsync(CompareQuery query, CancellationToken cancellationToken = default)
    {
        return _api.Get().ToAsync<CompareResults>(BuildCompareUrl(query), cancellationToken);
    }

    public Commit GetCommit(Sha1 sha) => _api.Get().To<Commit>(_repoPath + "/commits/" + sha);

    public IEnumerable<Diff> GetCommitDiff(Sha1 sha) => _api.Get().GetAll<Diff>(_repoPath + "/commits/" + sha + "/diff");

    public IEnumerable<Ref> GetCommitRefs(Sha1 sha, CommitRefType type = CommitRefType.All) =>
        _api.Get().GetAll<Ref>($"{_repoPath}/commits/{sha}/refs?type={type.ToString().ToLowerInvariant()}");

    public IFilesClient Files => new FilesClient(_api, _repoPath);

    public IBranchClient Branches => new BranchClient(_api, _repoPath);

    public IProjectHooksClient ProjectHooks => new ProjectHooksClient(_api, _projectPath);

    private string BuildGetTreeUrl(RepositoryGetTreeOptions options)
    {
        var url = $"{_repoPath}/tree";
        url = Utils.AddParameter(url, "path", options.Path);
        url = Utils.AddParameter(url, "ref", options.Ref);
        url = Utils.AddParameter(url, "recursive", options.Recursive);
        url = Utils.AddParameter(url, "per_page", options.PerPage);

        return url;
    }

    private string BuildCompareUrl(CompareQuery query)
    {
        var url = _repoPath + $"/compare?from={query.Source}&to={query.Target}";
        url = Utils.AddParameter(url, "from_project_id", query.FromProjectId);
        url = Utils.AddParameter(url, "straight", query.Straight);
        url = Utils.AddParameter(url, "unidiff", query.Unidiff);
        return url;
    }
}
