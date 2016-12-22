using System.Collections.Generic;
using System.Runtime.Serialization;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class RunnerClient : IRunnerClient
    {
        private readonly API _api;

        public RunnerClient(API api)
        {
            _api = api;
        }

        public IEnumerable<Runner> Accessible => _api.Get().GetAll<Runner>(Runner.Url);

        public IEnumerable<Runner> All => _api.Get().GetAll<Runner>(Runner.Url + "/all");

        public Runner this[int id] => _api.Get().To<Runner>(Runner.Url + "/" + id);

        public void Delete(Runner runner) => Delete(runner.Id);

        public void Delete(int runnerId)
        {
            _api.Delete().To<Runner>(Runner.Url + "/" + runnerId);
        }

        public Runner Update(int runnerId, RunnerUpdate runnerUpdate)
        {
            string url = $"{Runner.Url}/{runnerId}";
            return _api.Put().With(runnerUpdate).To<Runner>(url);
        }

        IEnumerable<Runner> IRunnerClient.GetAvailableRunners(int projectId)
        {
            string url = $"{Project.Url}/{projectId}/runners";
            return _api.Get().GetAll<Runner>(url);
        }

        public Runner EnableRunner(int projectId, RunnerId runnerId)
        {
            string url = $"{Project.Url}/{projectId}/runners";
            return _api.Post().With(runnerId).To<Runner>(url);
        }

        public Runner DisableRunner(int projectId, RunnerId runnerId)
        {
            string url = $"{Project.Url}/{projectId}/runners/{runnerId.Id}";
            return _api.Delete().To<Runner>(url);
        }
    }
}