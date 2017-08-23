using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab {
    public interface INamespaceClient {
        /// <summary>
        ///     Get a list of projects accessible by the authenticated user.
        /// </summary>
        IEnumerable<Namespace> Accessible();

        Namespace Get(int id);

        Namespace Create(NamespaceCreate group);

        void Delete(int id);
    }
}