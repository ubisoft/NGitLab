using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    /// <summary>
    /// Usernames and groupnames fall under a special category called namespaces.
    /// https://docs.gitlab.com/ce/api/namespaces.html
    /// </summary>
    public interface INamespacesClient
    {
        /// <summary>
        /// Get a list of namespaces (group or users) accessible by the authenticated user.
        /// </summary>
        IEnumerable<Namespace> Accessible { get; }

        IEnumerable<Namespace> Search(string search);
    }
}