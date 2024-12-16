using System;
using System.Collections.Generic;
using System.IO;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

public class SnippetClient : ISnippetClient
{
    private const string ProjectUrl = "/projects";
    private const string SnippetUrl = "/snippets";

    private readonly API _api;

    public SnippetClient(API api)
    {
        _api = api;
    }

    public IEnumerable<Snippet> User => _api.Get().GetAll<Snippet>(SnippetUrl); // all snippet of the user

    public IEnumerable<Snippet> All => _api.Get().GetAll<Snippet>(SnippetUrl + "/public"); // all public snippets

    public IEnumerable<Snippet> ForProject(long projectId)
    {
        return _api.Get().GetAll<Snippet>($"{ProjectUrl}/{projectId.ToStringInvariant()}/snippets");
    }

    public Snippet Get(long projectId, long snippetId)
    {
        return _api.Get().To<Snippet>($"{ProjectUrl}/{projectId.ToStringInvariant()}/snippets/{snippetId.ToStringInvariant()}");
    }

    public void Create(SnippetCreate snippet)
    {
        _api.Post().With(snippet).To<SnippetCreate>(SnippetUrl);
    }

    public void Create(SnippetProjectCreate snippet)
    {
        _api.Post().With(snippet).To<SnippetProjectCreate>($"{ProjectUrl}/{snippet.ProjectId.ToStringInvariant()}/snippets");
    }

    public void Update(SnippetUpdate snippet)
    {
        _api.Put().With(snippet).To<SnippetUpdate>($"{SnippetUrl}/{snippet.SnippetId.ToStringInvariant()}");
    }

    public void Update(SnippetProjectUpdate snippet)
    {
        _api.Put().With(snippet).To<SnippetProjectUpdate>($"{ProjectUrl}/{snippet.ProjectId.ToStringInvariant()}/snippets/{snippet.SnippetId.ToStringInvariant()}");
    }

    public void Delete(long snippetId) => _api.Delete().Execute($"{SnippetUrl}/{snippetId.ToStringInvariant()}");

    public void Delete(long projectId, long snippetId) => _api.Delete().Execute($"{ProjectUrl}/{projectId.ToStringInvariant()}/snippets/{snippetId.ToStringInvariant()}");

    public void GetContent(long projectId, long snippetId, Action<Stream> parser)
        => _api.Get().Stream($"{ProjectUrl}/{projectId.ToStringInvariant()}/snippets/{snippetId.ToStringInvariant()}/raw", parser);
}
