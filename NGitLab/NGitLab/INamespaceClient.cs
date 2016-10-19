using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface INamespaceClient
    {
        /// <summary>
        /// Get a list of projects accessible by the authenticated user.
        /// </summary>
        IEnumerable<Namespace> Accessible { get; }

        Namespace this[int id] { get; }

        Namespace Create(NamespaceCreate group);

        /// <summary>
        /// Returns the members of a namespace.
        /// </summary>
        /// <param name="namespaceId">The id or name of the namespace.</param>
        IMembersClient GetMembers(string namespaceId);

        void Delete(int id);
    }
}