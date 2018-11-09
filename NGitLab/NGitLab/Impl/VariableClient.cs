using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{
    internal abstract class VariableClient
    {
        private readonly string _urlPrefix;
        private readonly API _api;

        protected VariableClient(API api, string urlPrefix)
        {
            _urlPrefix = urlPrefix;
            _api = api;
        }

        public IEnumerable<Variable> All => _api.Get().GetAll<Variable>(_urlPrefix + "/variables");
        public Variable this[string key] => _api.Get().To<Variable>(_urlPrefix + "/variables/" + key);
        public Variable Create(VariableCreate model) => _api.Post().With(model).To<Variable>(_urlPrefix + "/variables");
        public Variable Update(string key, VariableUpdate model) => _api.Put().With(model).To<Variable>(_urlPrefix + "/variables/" + key);
        public void Delete(string key) => _api.Delete().Execute(_urlPrefix + "/variables/" + key);
    }
}
