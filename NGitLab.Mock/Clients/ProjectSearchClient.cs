using System;
using System.Collections.Generic;
using System.IO;
using NGitLab.Extensions;
using NGitLab.Mock.Internals;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class ProjectSearchClient : ClientBase, ISearchClient
{
    private readonly ClientContext _context;
    private readonly long _projectId;

    public ProjectSearchClient(ClientContext context, ProjectId projectId)
        : base(context)
    {
        _context = context;
        _projectId = Server.AllProjects.FindProject(projectId.ValueAsString()).Id;
    }

    public GitLabCollectionResponse<SearchBlob> GetBlobsAsync(SearchQuery query)
    {
        if (query.Search.StartsWith("filename:", StringComparison.Ordinal))
            return SearchByFilename(query);

        if (query.Search.StartsWith("path:", StringComparison.Ordinal))
            return SearchByPath(query);

        throw new NotImplementedException();
    }

    private GitLabCollectionResponse<SearchBlob> SearchByPath(SearchQuery query)
    {
        var path = query.Search["path:".Length..];
        var project = _context.Server.AllProjects.FindProject(_projectId.ToStringInvariant());
        var rootUri = new Uri(project.HttpUrl.TrimEnd('/') + '/');
        var expectedUri = new Uri(rootUri, path);
        var expectedFilePath = expectedUri.LocalPath;

        if (!System.IO.File.Exists(expectedFilePath))
            return GitLabCollectionResponse.Create(Array.Empty<SearchBlob>());

        return GitLabCollectionResponse.Create([
            new SearchBlob
            {
                FileName = Path.GetFileName(expectedFilePath),
                Ref = project.DefaultBranch,
                ProjectId = project.Id,
                Path = rootUri.MakeRelativeUri(expectedUri).ToString(),
                Data = System.IO.File.ReadAllText(expectedFilePath),
            },
        ]);
    }

    private GitLabCollectionResponse<SearchBlob> SearchByFilename(SearchQuery query)
    {
        var filename = query.Search["filename:".Length..];
        var project = _context.Server.AllProjects.FindProject(_projectId.ToStringInvariant());
        var rootUri = new Uri(project.HttpUrl.TrimEnd('/') + '/');
        var files = Directory.GetFiles(rootUri.LocalPath, filename, searchOption: SearchOption.AllDirectories);

        if (files.Length == 0)
            return GitLabCollectionResponse.Create(Array.Empty<SearchBlob>());

        var blobs = new List<SearchBlob>();
        foreach (var file in files)
        {
            var fileUri = new Uri(file);
            blobs.Add(new SearchBlob
            {
                FileName = Path.GetFileName(file),
                Ref = project.DefaultBranch,
                ProjectId = project.Id,
                Path = rootUri.MakeRelativeUri(fileUri).ToString(),
                Data = System.IO.File.ReadAllText(file),
            });
        }

        return GitLabCollectionResponse.Create(blobs);
    }
}
