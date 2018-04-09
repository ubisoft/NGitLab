using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class TriggerClient : ITriggerClient
    {
        private readonly API _api;
        private readonly string _triggersPath;

        public TriggerClient(API api, int projectId)
        {
            _api = api;
            _triggersPath = $"{Project.Url}/{projectId}/triggers";
        }

        public Trigger this[int id] => _api.Get().To<Trigger>(_triggersPath + "/" + id);

        public IEnumerable<Trigger> All => _api.Get().GetAll<Trigger>(_triggersPath);

        public Trigger Create(string description)
        {
            return _api.Post().To<Trigger>($"{_triggersPath}?description={description}");
        }
    }
}
