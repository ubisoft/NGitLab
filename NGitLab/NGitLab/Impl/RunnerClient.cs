using System.Collections.Generic;
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
    }
}