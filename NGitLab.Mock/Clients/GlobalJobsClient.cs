using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NGitLab.Mock.Clients;

internal sealed class GlobalJobsClient : ClientBase, IGlobalJobClient
{
    public GlobalJobsClient(ClientContext context)
        : base(context)
    {
    }

    async Task<Models.Job> IGlobalJobClient.GetJobFromJobTokenAsync(string token, CancellationToken cancellationToken)
    {
        await Task.Yield();
        using (Context.BeginOperationScope())
        {
            var job = Server.AllProjects.SelectMany(p => p.Jobs).FirstOrDefault(j => string.Equals(j.JobToken, token, StringComparison.Ordinal));

            if (job == null)
                throw new GitLabNotFoundException();

            return job.ToJobClient();
        }
    }
}
