using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class NamespacesClient : INamespacesClient
    {
        private readonly API _api;

        public const string Url = "/namespaces";


        public NamespacesClient(API api)
        {
            _api = api;
        }

        public IEnumerable<Namespace> Accessible => _api.Get().GetAll<Namespace>(Url);

        public IEnumerable<Namespace> Search(string search)
        {
            return _api.Get().GetAll<Namespace>(Url + $"?search={search}");
        }
    }
}