﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

public class JobClient : IJobClient
{
    private readonly API _api;
    private readonly string _jobsPath;

    public JobClient(API api, ProjectId projectId)
    {
        _api = api;
        _jobsPath = $"{Project.Url}/{projectId.ValueAsUriParameter()}/jobs";
    }

    public IEnumerable<Job> GetJobs(JobScopeMask scope)
    {
        return GetJobs(new JobQuery { Scope = scope });
    }

    public GitLabCollectionResponse<Job> GetJobsAsync(JobQuery query)
    {
        var url = CreateGetJobsUrl(query);
        return _api.Get().GetAllAsync<Job>(url);
    }

    public IEnumerable<Job> GetJobs(JobQuery query)
    {
        var url = CreateGetJobsUrl(query);
        return _api.Get().GetAll<Job>(url);
    }

    private string CreateGetJobsUrl(JobQuery query)
    {
        var url = _jobsPath;

        if (query.Scope != JobScopeMask.All)
        {
            foreach (Enum value in Enum.GetValues(query.Scope.GetType()))
            {
                if (query.Scope.HasFlag(value))
                {
                    url = Utils.AddParameter(url, "scope[]", value.ToString().ToLowerInvariant());
                }
            }
        }

        if (query.PerPage != null)
            url = Utils.AddParameter(url, "per_page", query.PerPage);
        return url;
    }

    public Job RunAction(long jobId, JobAction action) => _api.Post().To<Job>($"{_jobsPath}/{jobId.ToStringInvariant()}/{action.ToString().ToLowerInvariant()}");

    public Task<Job> RunActionAsync(long jobId, JobAction action, CancellationToken cancellationToken = default)
    {
        return _api.Post().ToAsync<Job>($"{_jobsPath}/{jobId.ToStringInvariant()}/{action.ToString().ToLowerInvariant()}", cancellationToken);
    }

    public Job Get(long jobId) => _api.Get().To<Job>($"{_jobsPath}/{jobId.ToStringInvariant()}");

    public Task<Job> GetAsync(long jobId, CancellationToken cancellationToken = default)
    {
        return _api.Get().ToAsync<Job>($"{_jobsPath}/{jobId.ToStringInvariant()}", cancellationToken);
    }

    public byte[] GetJobArtifacts(long jobId)
    {
        byte[] result = null;
        _api.Get().Stream($"{_jobsPath}/{jobId.ToStringInvariant()}/artifacts", s =>
        {
            using var ms = new MemoryStream();
            s.CopyTo(ms);
            result = ms.ToArray();
        });
        return result;
    }

    public Task DeleteJobArtifactsAsync(long jobId, CancellationToken cancellationToken = default)
    {
        return _api.Delete().ExecuteAsync($"{_jobsPath}/{jobId.ToStringInvariant()}/artifacts", cancellationToken);
    }

    public byte[] GetJobArtifact(long jobId, string path)
    {
        byte[] result = null;
        _api.Get().Stream($"{_jobsPath}/{jobId.ToStringInvariant()}/artifacts/{path}", s =>
        {
            using var ms = new MemoryStream();
            s.CopyTo(ms);
            result = ms.ToArray();
        });
        return result;
    }

    public byte[] GetJobArtifact(JobArtifactQuery query)
    {
        byte[] result = null;
        _api.Get().Stream($"{_jobsPath}/artifacts/{query.RefName}/raw/{query.ArtifactPath}?job={query.JobName}", s =>
        {
            using var ms = new MemoryStream();
            s.CopyTo(ms);
            result = ms.ToArray();
        });
        return result;
    }

    public string GetTrace(long jobId)
    {
        var result = string.Empty;
        _api.Get().Stream($"{_jobsPath}/{jobId.ToStringInvariant()}/trace", s =>
        {
            result = new StreamReader(s).ReadToEnd();
        });
        return result;
    }

    public async Task<string> GetTraceAsync(long jobId, CancellationToken cancellationToken = default)
    {
        var result = string.Empty;
        await _api.Get().StreamAsync($"{_jobsPath}/{jobId.ToStringInvariant()}/trace", async s =>
        {
            using var sr = new StreamReader(s);
            result = await sr.ReadToEndAsync().ConfigureAwait(false);
        }, cancellationToken).ConfigureAwait(false);
        return result;
    }
}
