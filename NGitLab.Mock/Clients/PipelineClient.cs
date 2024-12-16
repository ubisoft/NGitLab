using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Mock.Internals;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class PipelineClient : ClientBase, IPipelineClient
{
    private readonly long _projectId;
    private readonly IJobClient _jobClient;

    public PipelineClient(ClientContext context, IJobClient jobClient, ProjectId projectId)
        : base(context)
    {
        _jobClient = jobClient;
        _projectId = Server.AllProjects.FindProject(projectId.ValueAsString()).Id;
    }

    public Models.Pipeline this[long id]
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                var pipeline = project.Pipelines.GetById(id);
                if (pipeline == null)
                    throw new GitLabNotFoundException();

                return pipeline.ToPipelineClient();
            }
        }
    }

    public IEnumerable<PipelineBasic> All
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                return project.Pipelines.Select(p => p.ToPipelineBasicClient()).ToList();
            }
        }
    }

    public IEnumerable<Models.Job> AllJobs => _jobClient.GetJobs(JobScopeMask.All);

    public Models.Pipeline Create(string @ref)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            var pipeline = project.Pipelines.Add(@ref, JobStatus.Running, Context.User);
            return pipeline.ToPipelineClient();
        }
    }

    public Models.Pipeline Create(PipelineCreate createOptions)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            var pipeline = project.Pipelines.Add(createOptions.Ref, JobStatus.Running, Context.User);
            pipeline.Variables = createOptions.Variables.Select(v => new PipelineVariable { Key = v.Key, Value = v.Value });
            return pipeline.ToPipelineClient();
        }
    }

    public Models.Pipeline CreatePipelineWithTrigger(string token, string @ref, Dictionary<string, string> variables)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            var pipeline = project.Pipelines.Add(@ref, JobStatus.Running, Context.User);
            pipeline.Variables = variables.Select(v => new PipelineVariable { Key = v.Key, Value = v.Value });
            pipeline.CiToken = token;
            return pipeline.ToPipelineClient();
        }
    }

    public void Delete(long pipelineId)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            var pipeline = project.Pipelines.GetById(pipelineId);
            project.Pipelines.Remove(pipeline);
        }
    }

    public IEnumerable<PipelineVariable> GetVariables(long pipelineId)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            var pipeline = project.Pipelines.GetById(pipelineId);
            return pipeline.Variables;
        }
    }

    public TestReport GetTestReports(long pipelineId)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            var pipeline = project.Pipelines.GetById(pipelineId);
            return pipeline.TestReports;
        }
    }

    public TestReportSummary GetTestReportsSummary(long pipelineId)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            var pipeline = project.Pipelines.GetById(pipelineId);
            return pipeline.TestReportsSummary;
        }
    }

    public GitLabCollectionResponse<Bridge> GetBridgesAsync(PipelineBridgeQuery query)
    {
        return GitLabCollectionResponse.Create(GetBridges(query));
    }

    public IEnumerable<Bridge> GetBridges(PipelineBridgeQuery query)
    {
        using (Context.BeginOperationScope())
        {
            var project = Context.Server.AllProjects.FindById(_projectId);
            var bridges = project.Jobs.Where(j => j.Pipeline.Id == query.PipelineId && j.DownstreamPipeline != null).Select(j => j.ToBridgeClient());
            return (query.Scope == null || query.Scope.Length == 0) ? bridges : bridges.Where(j => query.Scope.Contains(j.Status.ToString(), StringComparer.Ordinal));
        }
    }

    public Models.Job[] GetJobs(long pipelineId)
    {
        using (Context.BeginOperationScope())
        {
            return _jobClient.GetJobs(JobScopeMask.All).Where(p => p.Pipeline.Id == pipelineId).ToArray();
        }
    }

    public IEnumerable<Models.Job> GetJobs(PipelineJobQuery query)
    {
        using (Context.BeginOperationScope())
        {
            var jobs = _jobClient.GetJobs(JobScopeMask.All).Where(j => j.Pipeline.Id == query.PipelineId);
            return (query.Scope == null || query.Scope.Length == 0) ? jobs : jobs.Where(j => query.Scope.Contains(j.Status.ToString(), StringComparer.Ordinal));
        }
    }

    [Obsolete("Use JobClient.GetJobs() instead")]
    public IEnumerable<Models.Job> GetJobsInProject(JobScope scope)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<PipelineBasic> Search(PipelineQuery query)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            IEnumerable<Pipeline> pipelines = project.Pipelines;
            if (query.Sha != null)
            {
                var sha = new Sha1(query.Sha);
                pipelines = pipelines.Where(pipeline => pipeline.Sha.Equals(sha));
            }

            if (query.Name != null)
            {
                pipelines = pipelines.Where(pipeline => string.Equals(pipeline.User.Name, query.Name, StringComparison.Ordinal));
            }

            if (query.Ref != null)
            {
                pipelines = pipelines.Where(pipeline => string.Equals(pipeline.Ref, query.Ref, StringComparison.Ordinal));
            }

            if (query.Scope.HasValue)
            {
                if (query.Scope.Value == PipelineScope.tags)
                {
                    pipelines = pipelines.Where(p => p.Tag);
                }
                else if (query.Scope.Value == PipelineScope.branches)
                {
                    pipelines = pipelines.Where(p => !p.Tag);
                }
                else if (query.Scope.Value == PipelineScope.running)
                {
                    pipelines = pipelines.Where(p => p.Status == JobStatus.Running);
                }
                else if (query.Scope.Value == PipelineScope.pending)
                {
                    pipelines = pipelines.Where(p => p.Status == JobStatus.Pending);
                }
                else if (query.Scope.Value == PipelineScope.finished)
                {
                    pipelines = pipelines.Where(p => p.FinishedAt.HasValue);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            if (query.Status.HasValue)
            {
                pipelines = pipelines.Where(pipeline => pipeline.Status == query.Status);
            }

            if (query.Username != null)
            {
                pipelines = pipelines.Where(pipeline => string.Equals(pipeline.User.UserName, query.Username, StringComparison.Ordinal));
            }

            if (query.YamlErrors.HasValue)
            {
                pipelines = pipelines.Where(pipeline => !string.IsNullOrEmpty(pipeline.YamlError));
            }

            if (query.OrderBy.HasValue)
            {
                pipelines = query.OrderBy.Value switch
                {
                    PipelineOrderBy.id => QuerySort(pipelines, query.Sort, p => p.Id),
                    PipelineOrderBy.status => QuerySort(pipelines, query.Sort, p => p.Status),
                    PipelineOrderBy.@ref => QuerySort(pipelines, query.Sort, p => p.Ref),
                    PipelineOrderBy.user_id => QuerySort(pipelines, query.Sort, p => p.User.Id),
                    PipelineOrderBy.updated_at => QuerySort(pipelines, query.Sort, p => p.UpdatedAt),
                    _ => throw new NotImplementedException(),
                };
            }
            else
            {
                pipelines = QuerySort(pipelines, query.Sort, p => p.UpdatedAt);
            }

            return pipelines.Where(p => !p.IsDownStreamPipeline).Select(pipeline => pipeline.ToPipelineBasicClient()).ToList();
        }
    }

    private static IEnumerable<Pipeline> QuerySort<T>(IEnumerable<Pipeline> pipelines, PipelineSort? sort, Func<Pipeline, T> expression)
    {
        if (!sort.HasValue)
            sort = PipelineSort.desc;

        if (sort.Value == PipelineSort.desc)
            return pipelines.OrderByDescending(expression);

        return pipelines.OrderBy(expression);
    }

    public async Task<Models.Pipeline> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return this[id];
    }

    public GitLabCollectionResponse<Models.Job> GetAllJobsAsync()
    {
        return GitLabCollectionResponse.Create(AllJobs);
    }

    public GitLabCollectionResponse<Models.Job> GetJobsAsync(PipelineJobQuery query)
    {
        return GitLabCollectionResponse.Create(GetJobs(query));
    }

    [SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "Would be an infinite recursion")]
    public async Task<Models.Pipeline> CreateAsync(PipelineCreate createOptions, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return Create(createOptions);
    }

    public GitLabCollectionResponse<PipelineBasic> SearchAsync(PipelineQuery query)
    {
        return GitLabCollectionResponse.Create(Search(query));
    }

    public GitLabCollectionResponse<PipelineVariable> GetVariablesAsync(long pipelineId)
    {
        return GitLabCollectionResponse.Create(GetVariables(pipelineId));
    }

    public Task<Models.Pipeline> RetryAsync(long pipelineId, CancellationToken cancellationToken = default)
    {
        using (Context.BeginOperationScope())
        {
            var jobs = _jobClient.GetJobs(JobScopeMask.Failed).Where(j => j.Pipeline.Id == pipelineId);
            foreach (var job in jobs)
            {
                _jobClient.RunAction(job.Id, JobAction.Retry);
            }

            return Task.FromResult(this[pipelineId]);
        }
    }

    public Task<Models.Pipeline> UpdateMetadataAsync(long pipelineId, PipelineMetadataUpdate update, CancellationToken cancellationToken = default)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.Edit);
            var pipeline = project.Pipelines.GetById(pipelineId);

            // Currently, "name" is the only field that can be updated via the "Update pipeline metadata" API
            // See https://docs.gitlab.com/ee/api/pipelines.html#update-pipeline-metadata
            // If no name is set, GitLab returns a "Bad Request" error
            // This behavior is mimicked here
            if (update.Name is not null)
            {
                pipeline.Name = update.Name;
            }
            else
            {
                throw new GitLabBadRequestException("name is missing");
            }

            return Task.FromResult(pipeline.ToPipelineClient());
        }
    }
}
