using System.Collections.Generic;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl
{
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

        public IEnumerable<Snippet> ForProject(int projectId)
        {
            return _api.Get().GetAll<Snippet>($"{ProjectUrl}/{projectId.ToStringInvariant()}/snippets");
        }

        public Snippet Get(int projectId, int snippetId)
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

        public void Delete(int snippetId) => _api.Delete().Execute($"{SnippetUrl}/{snippetId.ToStringInvariant()}");

        public void Delete(int projectId, int snippetId) => _api.Delete().Execute($"{ProjectUrl}/{projectId.ToStringInvariant()}/snippets/{snippetId.ToStringInvariant()}");
    }
}
