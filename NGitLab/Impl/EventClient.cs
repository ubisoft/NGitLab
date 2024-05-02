using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl;

public class EventClient : IEventClient
{
    private readonly API _api;
    private readonly string _baseUrl;

    public EventClient(API api, string baseUrl)
    {
        _api = api;
        _baseUrl = baseUrl;
    }

    public IEnumerable<Event> Get(EventQuery query)
    {
        var url = _baseUrl;

        url = Utils.AddParameter(url, "action", query.Action);
        url = Utils.AddParameter(url, "target_type", query.Type);
        url = Utils.AddParameter(url, "before", query.Before?.Date);
        url = Utils.AddParameter(url, "after", query.After?.Date);
        url = Utils.AddParameter(url, "scope", query.Scope);
        url = Utils.AddParameter(url, "sort", query.Sort);
        url = Utils.AddParameter(url, "per_page", query.PerPage);
        url = Utils.AddParameter(url, "page", query.Page);

        return _api.Get().GetAll<Event>(url);
    }
}
