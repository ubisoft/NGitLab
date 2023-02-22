using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{
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
            var url = QueryStringHelper.BuildAndAppendQueryString(_baseUrl, query);
            return _api.Get().GetAll<Event>(url);
        }
    }
}
