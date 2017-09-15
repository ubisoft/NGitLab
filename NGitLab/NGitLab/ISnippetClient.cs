using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab
{
    public interface ISnippetClient
    {

        /// <summary>
        /// Get a list of snippets for the specified project.
        /// 
        /// url like GET /projects/:id/snippets
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        IEnumerable<Snippet> ForProject(int projectId);

        /// <summary>
        /// Return a single snippet for a given project.
        /// 
        /// url like GET /projects/:id/snippets/:snippet_id
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="snippetId"></param>
        /// <returns></returns>
        Snippet Get(int projectId, int snippetId);
    }
}
