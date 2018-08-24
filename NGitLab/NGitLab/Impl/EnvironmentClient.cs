using System;
using System.Collections.Generic;
using NGitLab.Models;
using Environment = NGitLab.Models.Environment;

namespace NGitLab.Impl
{
    public class EnvironmentClient : IEnvironmentClient
    {
        private readonly API _api;
        private readonly string _environmentsPath;

        public EnvironmentClient(API api, int projectId)
        {
            _api = api;
            _environmentsPath = $"{Project.Url}/{projectId}/environments";
        }

        public IEnumerable<Environment> All => _api.Get().GetAll<Environment>(_environmentsPath);

        public Environment Create(string name, string externalUrl)
        {

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException();

            string url = Utils.AddParameter(_environmentsPath, "name", name);

            if (!string.IsNullOrEmpty(externalUrl))
                url = Utils.AddParameter(url, "external_url", externalUrl);

            return _api.Post().To<Environment>(url);
        }

        public Environment Edit(int environmentId, string name, string externalUrl)
        {
            string url = $"{_environmentsPath}/{environmentId}";

            if (!string.IsNullOrEmpty(name))
                url = Utils.AddParameter(url, "name", name);

            if (!string.IsNullOrEmpty(externalUrl))
                url = Utils.AddParameter(url, "external_url", externalUrl);

            return _api.Put().To<Environment>(url);
        }

        public void Delete(int environmentId) => _api.Delete().Execute($"{_environmentsPath}/{environmentId}");

        public Environment Stop(int environmentId)
        {
            return _api.Post().To<Environment>($"{_environmentsPath}/{environmentId}/stop");
        }
    }
}
