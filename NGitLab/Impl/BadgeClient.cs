using System.Collections.Generic;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl
{
    internal abstract class BadgeClient
    {
        private readonly string _urlPrefix;
        private readonly API _api;

        protected BadgeClient(API api, string urlPrefix)
        {
            _urlPrefix = urlPrefix;
            _api = api;
        }

        public IEnumerable<Badge> All => _api.Get().GetAll<Badge>(_urlPrefix + "/badges");

        public Badge this[int id] => _api.Get().To<Badge>(_urlPrefix + "/badges/" + id.ToStringInvariant());

        public Badge Create(BadgeCreate badge) => _api.Post().With(badge).To<Badge>(_urlPrefix + "/badges");

        public Badge Update(int id, BadgeUpdate badge) => _api.Put().With(badge).To<Badge>(_urlPrefix + "/badges/" + id.ToStringInvariant());

        public void Delete(int id) => _api.Delete().Execute(_urlPrefix + "/badges/" + id.ToStringInvariant());
    }
}
