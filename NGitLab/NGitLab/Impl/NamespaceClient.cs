using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class NamespaceClient : INamespaceClient
    {
        private readonly API _api;

        public NamespaceClient(API api)
        {
            _api = api;
        }

        public IEnumerable<Namespace> Accessible()
        {
            return _api.Get().GetAll<Namespace>(Namespace.Url);
        }

        public Namespace Get(int id)
        {
            return _api.Get().To<Namespace>(Namespace.Url + "/" + id);
        }

        public Namespace Create(NamespaceCreate group)
        {
            return _api.Post().With(group).To<Namespace>(Namespace.Url);
        }

        public void Delete(int id)
        {
            _api.Delete().To<Namespace>(Namespace.Url + "/" + id);
        }
    }
}