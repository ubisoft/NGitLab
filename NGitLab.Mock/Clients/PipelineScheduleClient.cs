using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Mock.Internals;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;
internal class PipelineScheduleClient : ClientBase, IPipelineScheduleClient
{
    private readonly ProjectId _projectId;

    public PipelineScheduleClient(ClientContext context, ProjectId projectId)
        : base(context)
    {
        _projectId = projectId;
    }

    public Models.PipelineSchedule this[int id]
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                var schedule = project.PipelineSchedules.GetById(id);
                if (schedule == null)
                    throw new GitLabNotFoundException();

                return schedule.ToPipelineScheduleClient();
            }
        }
    }

    public IEnumerable<PipelineScheduleBasic> All
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                return project.PipelineSchedules.Select(s => s.ToPipelineScheduleBasicClient()).ToList();
            }
        }
    }

    public GitLabCollectionResponse<PipelineScheduleBasic> GetAllAsync(CancellationToken cancellationToken = default)
        => GitLabCollectionResponse.Create(All);

    public async Task<Models.PipelineSchedule> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        await Task.Yield();

        return this[id];
    }
}
