using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

public class RunnerClient : IRunnerClient
{
    private readonly API _api;

    public RunnerClient(API api)
    {
        _api = api;
    }

    public IEnumerable<Runner> Accessible => _api.Get().GetAll<Runner>(Runner.Url);

    public IEnumerable<Runner> All => _api.Get().GetAll<Runner>(Runner.Url + "/all");

    public Runner this[long id] => _api.Get().To<Runner>(Runner.Url + "/" + id.ToStringInvariant());

    public IEnumerable<Runner> GetAllRunnersWithScope(RunnerScope scope)
    {
        var url = Runner.Url + "/all";
        url = Utils.AddParameter(url, "scope", scope.ToString().ToLowerInvariant());
        return _api.Get().GetAll<Runner>(url);
    }

    public IEnumerable<Runner> OfProject(long projectId)
    {
        return _api.Get().GetAll<Runner>(Project.Url + $"/{projectId.ToStringInvariant()}" + Runner.Url);
    }

    public GitLabCollectionResponse<Runner> OfProjectAsync(long projectId)
    {
        return _api.Get().GetAllAsync<Runner>(Project.Url + $"/{projectId.ToStringInvariant()}" + Runner.Url);
    }

    public IEnumerable<Runner> OfGroup(long groupId)
    {
        return _api.Get().GetAll<Runner>(Group.Url + $"/{groupId.ToStringInvariant()}" + Runner.Url);
    }

    public GitLabCollectionResponse<Runner> OfGroupAsync(long groupId)
    {
        return _api.Get().GetAllAsync<Runner>(Group.Url + $"/{groupId.ToStringInvariant()}" + Runner.Url);
    }

    public void Delete(Runner runner) => Delete(runner.Id);

    public void Delete(long runnerId)
    {
        _api.Delete().Execute(Runner.Url + "/" + runnerId.ToStringInvariant());
    }

    public Runner Update(long runnerId, RunnerUpdate runnerUpdate)
    {
        var url = $"{Runner.Url}/{runnerId.ToStringInvariant()}";
        return _api.Put().With(runnerUpdate).To<Runner>(url);
    }

    public IEnumerable<Job> GetJobs(long runnerId, JobStatus? status = null)
    {
        var url = $"{Runner.Url}/{runnerId.ToStringInvariant()}/jobs";

        if (status != null)
        {
            url = Utils.AddParameter(url, "status", status.Value.ToString().ToLowerInvariant());
        }

        return _api.Get().GetAll<Job>(url);
    }

    IEnumerable<Runner> IRunnerClient.GetAvailableRunners(long projectId)
    {
        var url = $"{Project.Url}/{projectId.ToStringInvariant()}/runners";
        return _api.Get().GetAll<Runner>(url);
    }

    public Runner EnableRunner(long projectId, RunnerId runnerId)
    {
        var url = $"{Project.Url}/{projectId.ToStringInvariant()}/runners";
        return _api.Post().With(runnerId).To<Runner>(url);
    }

    public void DisableRunner(long projectId, RunnerId runnerId)
    {
        var url = $"{Project.Url}/{projectId.ToStringInvariant()}/runners/{runnerId.Id.ToStringInvariant()}";
        _api.Delete().Execute(url);
    }

    public Runner Register(RunnerRegister request)
    {
        return _api.Post().With(request).To<Runner>(Runner.Url);
    }

    public Task<Runner> EnableRunnerAsync(long projectId, RunnerId runnerId, CancellationToken cancellationToken = default)
    {
        var url = $"{Project.Url}/{projectId.ToStringInvariant()}/runners";
        return _api.Post().With(runnerId).ToAsync<Runner>(url, cancellationToken);
    }
}
