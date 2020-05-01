using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class SnippetClient : ClientBase, ISnippetClient
    {
        public SnippetClient(ClientContext context)
            : base(context)
        {
        }

        public IEnumerable<Snippet> All => throw new System.NotImplementedException();

        public IEnumerable<Snippet> User => throw new System.NotImplementedException();

        public void Create(SnippetCreate snippet)
        {
            throw new System.NotImplementedException();
        }

        public void Create(SnippetProjectCreate snippet)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int snippetId)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int projectId, int snippetId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Snippet> ForProject(int projectId)
        {
            throw new System.NotImplementedException();
        }

        public Snippet Get(int projectId, int snippetId)
        {
            throw new System.NotImplementedException();
        }
    }
}
