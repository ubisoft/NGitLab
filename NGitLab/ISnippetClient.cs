using System;
using System.Collections.Generic;
using System.IO;
using NGitLab.Models;

namespace NGitLab;

public interface ISnippetClient
{
    /// <summary>
    /// Get a list of snippets for the specified project.
    /// </summary>
    IEnumerable<Snippet> ForProject(long projectId);

    /// <summary>
    /// Return a single snippet for a given project.
    /// </summary>
    Snippet Get(long projectId, long snippetId);

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
    /// Update a user's snippet
    /// </summary>
    void Update(SnippetUpdate snippet);

    /// <summary>
    /// Update a project's snippet
    /// </summary>
    void Update(SnippetProjectUpdate snippet);

    /// <summary>
    /// Delete a snippet not linked to a project but only to a user, could delete snippet linked to a project but will return an error 403 in API v4
    /// </summary>
    void Delete(long snippetId);

    /// <summary>
    /// Delete a snippet linked to a project
    /// </summary>
    void Delete(long projectId, long snippetId);

    /// <summary>
    /// Get single snippet's content for a given project
    /// </summary>
    void GetContent(long projectId, long snippetId, Action<Stream> parser);
}
