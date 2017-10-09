using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab {
    public interface INamespaceClient {
        /// <summary>
        ///     Get a list of projects accessible by the authenticated user.
        /// </summary>
        IEnumerable<Namespace> Accessible();
        /// <summary>
        /// Usernames and groupnames fall under a special category called namespaces
        /// </summary>
        /// <seealso cref="https://docs.gitlab.com/ce/api/namespaces.html"/>
        /// <returns></returns>
        IEnumerable<Namespaces> GetNamespaces();
        Namespace Get(int id);

        Namespace Create(NamespaceCreate group);

        void Delete(int id);
    }
}