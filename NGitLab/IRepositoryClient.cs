﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

public interface IRepositoryClient
{
    ITagClient Tags { get; }

    IEnumerable<Tree> Tree { get; }

    IEnumerable<Tree> GetTree(string path);

    IEnumerable<Tree> GetTree(string path, string @ref, bool recursive);

    IEnumerable<Tree> GetTree(RepositoryGetTreeOptions options);

    GitLabCollectionResponse<Tree> GetTreeAsync(RepositoryGetTreeOptions options);

    void GetRawBlob(string sha, Action<Stream> parser);

    void GetArchive(Action<Stream> parser);

    IEnumerable<Commit> Commits { get; }

    IContributorClient Contributors { get; }

    /// <summary>
    /// Gets all the commits of the specified branch/tag.
    /// </summary>
    IEnumerable<Commit> GetCommits(string refName, int maxResults = 0);

    /// <summary>
    /// Gets all the commits of the specified branch/tag and path.
    /// </summary>
    IEnumerable<Commit> GetCommits(GetCommitsRequest request);

    Commit GetCommit(Sha1 sha);

    IEnumerable<Diff> GetCommitDiff(Sha1 sha);

    IEnumerable<Ref> GetCommitRefs(Sha1 sha, CommitRefType type = CommitRefType.All);

    IFilesClient Files { get; }

    IBranchClient Branches { get; }

    IProjectHooksClient ProjectHooks { get; }

    /// <summary>
    /// Compare two branches/tags/commit hashes
    /// </summary>
    CompareResults Compare(CompareQuery query);

    /// <inheritdoc cref="Compare"/>
    Task<CompareResults> CompareAsync(CompareQuery query, CancellationToken cancellationToken = default);
}
