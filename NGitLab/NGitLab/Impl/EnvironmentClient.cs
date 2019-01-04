using System;
using System.Collections.Generic;
using NGitLab.Models;

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

        public IEnumerable<EnvironmentInfo> All => _api.Get().GetAll<EnvironmentInfo>(_environmentsPath);

        public EnvironmentInfo Create(string name, string externalUrl)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException();

            var url = Utils.AddParameter(_environmentsPath, "name", name);

            if (!string.IsNullOrEmpty(externalUrl))
            {
                url = Utils.AddParameter(url, "external_url", externalUrl);
            }

            return _api.Post().To<EnvironmentInfo>(url);
        }

        public EnvironmentInfo Edit(int environmentId, string name, string externalUrl)
        {
            var url = $"{_environmentsPath}/{environmentId}";

            if (!string.IsNullOrEmpty(name))
            {
                url = Utils.AddParameter(url, "name", name);
            }

            if (!string.IsNullOrEmpty(externalUrl))
            {
                url = Utils.AddParameter(url, "external_url", externalUrl);
            }

            return _api.Put().To<EnvironmentInfo>(url);
        }

        public void Delete(int environmentId) => _api.Delete().Execute($"{_environmentsPath}/{environmentId}");

        public EnvironmentInfo Stop(int environmentId)
        {
            return _api.Post().To<EnvironmentInfo>($"{_environmentsPath}/{environmentId}/stop");
        }
    }
}
