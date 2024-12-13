using System.Collections.Generic;
using System.Net;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

public class NamespacesClient : INamespacesClient
{
    private readonly API _api;

    public const string Url = "/namespaces";

    public NamespacesClient(API api)
    {
        _api = api;
    }

    public IEnumerable<Namespace> Accessible => _api.Get().GetAll<Namespace>(Url);

    public Namespace this[long id] => _api.Get().To<Namespace>(Url + "/" + id.ToStringInvariant());

    public Namespace this[string fullPath] => _api.Get().To<Namespace>(Url + "/" + WebUtility.UrlEncode(fullPath));

    public IEnumerable<Namespace> Search(string search)
    {
        return _api.Get().GetAll<Namespace>(Url + $"?search={search}");
    }
}
