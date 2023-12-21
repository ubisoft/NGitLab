using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Impl;

internal sealed class GlobalJobsClient : IGlobalJobClient
{
    private readonly API _api;

    public GlobalJobsClient(API api)
    {
        _api = api;
    }

    public Task<Job> GetJobFromJobTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var url = "/job";
        url = Utils.AddParameter(url, "job_token", token);
        return _api.Get().ToAsync<Job>(url, cancellationToken);
    }
}
