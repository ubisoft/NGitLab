using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
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

        public Runner this[int id] => _api.Get().To<Runner>(Runner.Url + "/" + id.ToStringInvariant());

        public IEnumerable<Runner> GetAllRunnersWithScope(RunnerScope scope)
        {
            var url = Runner.Url + "/all";
            url = Utils.AddParameter(url, "scope", scope.ToString().ToLowerInvariant());
            return _api.Get().GetAll<Runner>(url);
        }

        public IEnumerable<Runner> OfProject(int projectId)
        {
            return _api.Get().GetAll<Runner>(Project.Url + $"/{projectId.ToStringInvariant()}" + Runner.Url);
        }

        public GitLabCollectionResponse<Runner> OfProjectAsync(int projectId)
        {
            return _api.Get().GetAllAsync<Runner>(Project.Url + $"/{projectId.ToStringInvariant()}" + Runner.Url);
        }

        public void Delete(Runner runner) => Delete(runner.Id);

        public void Delete(int runnerId)
        {
            _api.Delete().Execute(Runner.Url + "/" + runnerId.ToStringInvariant());
        }

        public Runner Update(int runnerId, RunnerUpdate runnerUpdate)
        {
            var url = $"{Runner.Url}/{runnerId.ToStringInvariant()}";
            return _api.Put().With(runnerUpdate).To<Runner>(url);
        }

        [Obsolete("Use GetJobs(int, JobStatus?) instead")]
        public IEnumerable<Job> GetJobs(int runnerId, JobScope scope)
        {
            var url = $"{Runner.Url}/{runnerId.ToStringInvariant()}/jobs";

            if (scope != JobScope.All)
            {
                url = Utils.AddParameter(url, "status", scope.ToString().ToLowerInvariant());
            }

            return _api.Get().GetAll<Job>(url);
        }

        [SuppressMessage("ApiDesign", "RS0027:Public API with optional parameter(s) should have the most parameters amongst its public overloads", Justification = "Keep compatibility")]
        public IEnumerable<Job> GetJobs(int runnerId, JobStatus? status = null)
        {
            var url = $"{Runner.Url}/{runnerId.ToStringInvariant()}/jobs";

            if (status != null)
            {
                url = Utils.AddParameter(url, "status", status.Value.ToString().ToLowerInvariant());
            }

            return _api.Get().GetAll<Job>(url);
        }

        IEnumerable<Runner> IRunnerClient.GetAvailableRunners(int projectId)
        {
            var url = $"{Project.Url}/{projectId.ToStringInvariant()}/runners";
            return _api.Get().GetAll<Runner>(url);
        }

        public Runner EnableRunner(int projectId, RunnerId runnerId)
        {
            var url = $"{Project.Url}/{projectId.ToStringInvariant()}/runners";
            return _api.Post().With(runnerId).To<Runner>(url);
        }

        public void DisableRunner(int projectId, RunnerId runnerId)
        {
            var url = $"{Project.Url}/{projectId.ToStringInvariant()}/runners/{runnerId.Id.ToStringInvariant()}";
            _api.Delete().Execute(url);
        }

        public Runner Register(RunnerRegister request)
        {
            return _api.Post().With(request).To<Runner>(Runner.Url);
        }

        public Task<Runner> EnableRunnerAsync(int projectId, RunnerId runnerId, CancellationToken cancellationToken = default)
        {
            var url = $"{Project.Url}/{projectId.ToStringInvariant()}/runners";
            return _api.Post().With(runnerId).ToAsync<Runner>(url, cancellationToken);
        }
    }
}
