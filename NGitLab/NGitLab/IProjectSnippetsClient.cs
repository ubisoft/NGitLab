using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IProjectSnippetsClient
    {
        IEnumerable<ProjectSnippet> All { get; }
        ProjectSnippet Create(ProjectSnippetInsert snippet);
        ProjectSnippet Update(ProjectSnippetUpdate toUpdate);
        void Delete(int snippetId);
        ProjectSnippet Get(int snippet_id);

    }
}