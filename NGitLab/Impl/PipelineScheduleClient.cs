using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

internal class PipelineScheduleClient : IPipelineScheduleClient
{
    private readonly API _api;

    private readonly string _projectPath;
    private readonly string _schedulesPath;

    public PipelineScheduleClient(API api, ProjectId projectId)
    {
        _api = api;
        _projectPath = $"{Project.Url}/{projectId.ValueAsUriParameter()}";
        _schedulesPath = $"{_projectPath}/pipeline_schedules";
    }

    public PipelineSchedule this[int id] => _api.Get().To<PipelineSchedule>($"{_schedulesPath}/{id.ToStringInvariant()}");

    public IEnumerable<PipelineScheduleBasic> All => _api.Get().GetAll<PipelineScheduleBasic>(_schedulesPath);

    public GitLabCollectionResponse<PipelineScheduleBasic> GetAllAsync()
        => _api.Get().GetAllAsync<PipelineScheduleBasic>(_schedulesPath);

    public GitLabCollectionResponse<PipelineBasic> GetAllSchedulePipelinesAsync(int id)
        => _api.Get().GetAllAsync<PipelineBasic>($"{_schedulesPath}/{id.ToStringInvariant()}/pipelines");

    public Task<PipelineSchedule> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _api.Get().ToAsync<PipelineSchedule>($"{_schedulesPath}/{id.ToStringInvariant()}", cancellationToken);
}
