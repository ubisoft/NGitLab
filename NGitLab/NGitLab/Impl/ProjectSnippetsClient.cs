using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl {
    public class ProjectSnippetsClient : IProjectSnippetsClient
    {
        readonly Api api;
        readonly string path;
        public ProjectSnippetsClient(Api api, string projectPath) {
            this.api = api;
            path = projectPath + "/snippets";
        }

        public IEnumerable<ProjectSnippet> All => api.Get().GetAll<ProjectSnippet>(path);
        public ProjectSnippet Get(int snippet_id) {
            return api.Get().To<ProjectSnippet>(path + "/" + snippet_id);
        }
        public ProjectSnippet Create(ProjectSnippetInsert snippet) {
            return api.Post().With(snippet).To<ProjectSnippet>(path);
        }

    

        public ProjectSnippet Update(ProjectSnippetUpdate toUpdate)
        {
            return api.Put().With(toUpdate).To<ProjectSnippet>(path + "/" + toUpdate.SnippetID);
        }

        public void Delete(int snippetId)
        {
            api.Delete().To<ProjectSnippet>(path + "/" + snippetId);
        }
    }
}