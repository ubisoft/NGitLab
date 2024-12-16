using System;
using System.Collections.Generic;
using System.IO;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class SnippetClient : ClientBase, ISnippetClient
{
    public SnippetClient(ClientContext context)
        : base(context)
    {
    }

    public IEnumerable<Snippet> All => throw new NotImplementedException();

    public IEnumerable<Snippet> User => throw new NotImplementedException();

    public void Create(SnippetCreate snippet)
    {
        throw new NotImplementedException();
    }

    public void Create(SnippetProjectCreate snippet)
    {
        throw new NotImplementedException();
    }

    public void Update(SnippetUpdate snippet)
    {
        throw new NotImplementedException();
    }

    public void Update(SnippetProjectUpdate snippet)
    {
        throw new NotImplementedException();
    }

    public void Delete(long snippetId)
    {
        throw new NotImplementedException();
    }

    public void Delete(long projectId, long snippetId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Snippet> ForProject(long projectId)
    {
        throw new NotImplementedException();
    }

    public Snippet Get(long projectId, long snippetId)
    {
        throw new NotImplementedException();
    }

    public void GetContent(long projectId, long snippetId, Action<Stream> parser)
    {
        throw new NotImplementedException();
    }
}
