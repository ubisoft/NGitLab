using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public IEnumerable<Snippet> All => _api.Get().GetAll<Snippet>(SnippetUrl); // all snippet of the user

        public IEnumerable<Snippet> ForProject(int projectId)
        {
            return _api.Get().GetAll<Snippet>($"{ProjectUrl}/{projectId}/snippets");
        }

        public Snippet Get(int projectId, int snippetId)
        {
            return _api.Get().To<Snippet>($"{ProjectUrl}/{projectId}/snippets/{snippetId}");
        }

        public void Create(SnippetCreate snippet)
        {
            _api.Post().With(snippet).To<SnippetCreate>(SnippetUrl);
        }

        public void Create(SnippetProjectCreate snippet)
        {
            _api.Post().With(snippet).To<SnippetProjectCreate>($"{ProjectUrl}/{snippet.Id}/snippets");
        }

        public void Delete(int snippetId) => _api.Delete().Execute($"{SnippetUrl}/{snippetId}");

        public void Delete(int projectId, int snippetId) => _api.Delete().Execute($"{ProjectUrl}/{projectId}/snippets/{snippetId}");
    }
}
