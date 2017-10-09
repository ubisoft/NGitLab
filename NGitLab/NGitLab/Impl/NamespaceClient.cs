using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl {
    public class NamespaceClient : INamespaceClient {
        readonly Api api;

        public NamespaceClient(Api api) {
            this.api = api;
        }

        public IEnumerable<Namespace> Accessible() {
            return api.Get().GetAll<Namespace>(Namespace.Url);
        }
        public IEnumerable<Namespaces> GetNamespaces()
        {
            return api.Get().GetAll<Namespaces>(Namespaces.Url);
        }
        public Namespace Get(int id) {
            return api.Get().To<Namespace>(Namespace.Url + "/" + id);
        }

        public Namespace Create(NamespaceCreate group) {
            return api.Post().With(group).To<Namespace>(Namespace.Url);
        }

        public void Delete(int id) {
            api.Delete().To<Namespace>(Namespace.Url + "/" + id);
        }

       
    }
}