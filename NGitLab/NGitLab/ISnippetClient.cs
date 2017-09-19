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

        /// <summary>
        /// Return all snippets of the authenticated user
        /// 
        /// url like GET /snippets
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<Snippet> All { get; }

        /// <summary>
        /// Create a new user's snippet 
        /// 
        /// url like POST /snippets
        /// 
        /// </summary>
        /// <returns></returns>
        void Create(SnippetCreate snippet);

        /// <summary>
        /// Create a new project's snippet 
        /// 
        /// url like POST /projects/:id/snippets
        /// 
        /// </summary>
        /// <returns></returns>
        void Create(SnippetProjectCreate snippet);

        /// <summary>
        ///Delete a snippet not linked to a project but only to a user, could delete snippet linked to a project but will return an error 403 in API v4
        /// 
        /// url like DELETE /snippets/:id/
        /// 
        /// </summary>
        /// <param name="snippetId"></param>
        /// <returns></returns>
        void Delete(int snippetId);

        /// <summary>
        ///Delete a snippet linked to a project
        /// 
        /// url like DELETE /projects/:id/snippets/:snippet_id
        /// 
        /// </summary>
        /// <param name="snippetId"></param>
        /// <returns></returns>
        void Delete(int projectId, int snippetId);
    }
}
