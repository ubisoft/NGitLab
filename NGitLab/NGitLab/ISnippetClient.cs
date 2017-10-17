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
        /// </summary>
        IEnumerable<Snippet> ForProject(int projectId);

        /// <summary>
        /// Return a single snippet for a given project.
        /// </summary>
        Snippet Get(int projectId, int snippetId);

        /// <summary>
        /// Return all snippets of the authenticated user
        /// </summary>
        IEnumerable<Snippet> All { get; }

        IEnumerable<Snippet> User { get; }

        /// <summary>
        /// Create a new user's snippet 
        /// </summary>
        void Create(SnippetCreate snippet);

        /// <summary>
        /// Create a new project's snippet 
        /// </summary>
        void Create(SnippetProjectCreate snippet);

        /// <summary>
        ///Delete a snippet not linked to a project but only to a user, could delete snippet linked to a project but will return an error 403 in API v4
        /// </summary>
        void Delete(int snippetId);

        /// <summary>
        ///Delete a snippet linked to a project
        /// </summary>
        void Delete(int projectId, int snippetId);
    }
}
