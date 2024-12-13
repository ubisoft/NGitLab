using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

public class PipelineClient : IPipelineClient
{
    private readonly API _api;
    private readonly string _projectPath;
    private readonly string _pipelinesPath;

    public PipelineClient(API api, ProjectId projectId)
    {
        _api = api;
        _projectPath = $"{Project.Url}/{projectId.ValueAsUriParameter()}";
        _pipelinesPath = $"{_projectPath}/pipelines";
    }

    public IEnumerable<PipelineBasic> All => _api.Get().GetAll<PipelineBasic>(_pipelinesPath);

    public IEnumerable<Job> AllJobs => _api.Get().GetAll<Job>($"{_projectPath}/jobs");

    public GitLabCollectionResponse<Job> GetAllJobsAsync()
    {
        return _api.Get().GetAllAsync<Job>($"{_projectPath}/jobs");
    }

    [Obsolete("Use JobClient.GetJobs() instead")]
    public IEnumerable<Job> GetJobsInProject(JobScope scope)
    {
        var url = $"{_projectPath}/jobs";

        if (scope != JobScope.All)
        {
            url = Utils.AddParameter(url, "scope", scope.ToString().ToLowerInvariant());
        }

        return _api.Get().GetAll<Job>(url);
    }

    public Pipeline this[long id] => _api.Get().To<Pipeline>($"{_pipelinesPath}/{id.ToStringInvariant()}");

    public Task<Pipeline> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return _api.Get().ToAsync<Pipeline>($"{_pipelinesPath}/{id.ToStringInvariant()}", cancellationToken);
    }

    public Job[] GetJobs(long pipelineId)
    {
        // For some reason GitLab returns the jobs in the reverse order as
        // they appear in their UI. Here we reverse them!
        var jobs = _api.Get().GetAll<Job>($"{_pipelinesPath}/{pipelineId.ToStringInvariant()}/jobs").Reverse().ToArray();
        return jobs;
    }

    public IEnumerable<Job> GetJobs(PipelineJobQuery query)
    {
        var url = CreateGetJobsUrl(query);

        // For some reason GitLab returns the jobs in the reverse order as
        // they appear in their UI. Here we reverse them!
        return _api.Get().GetAll<Job>(url).Reverse();
    }

    public GitLabCollectionResponse<Job> GetJobsAsync(PipelineJobQuery query)
    {
        var url = CreateGetJobsUrl(query);
        return _api.Get().GetAllAsync<Job>(url);
    }

    private string CreateGetJobsUrl(PipelineJobQuery query)
    {
        var url = $"{_pipelinesPath}/{query.PipelineId.ToStringInvariant()}/jobs";
        url = Utils.AddArrayParameter(url, "scope", query.Scope);
        url = Utils.AddParameter(url, "include_retried", query.IncludeRetried);
        return url;
    }

    public Pipeline Create(string @ref)
    {
        return _api.Post().To<Pipeline>($"{_projectPath}/pipeline?ref={@ref}");
    }

    public Pipeline Create(PipelineCreate createOptions)
    {
        var url = CreateCreateUrl(createOptions);
        return _api.Post().To<Pipeline>(url);
    }

    public Task<Pipeline> CreateAsync(PipelineCreate createOptions, CancellationToken cancellationToken = default)
    {
        var url = CreateCreateUrl(createOptions);
        return _api.Post().ToAsync<Pipeline>(url, cancellationToken);
    }

    private string CreateCreateUrl(PipelineCreate createOptions)
    {
        var variables = new StringBuilder();
        foreach (var variable in createOptions.Variables)
        {
            // see https://docs.gitlab.com/ee/api/#array-of-hashes
            variables
                .Append("&variables[][key]=").Append(Uri.EscapeDataString(variable.Key))
                .Append("&variables[][value]=").Append(Uri.EscapeDataString(variable.Value));
        }

        return $"{_projectPath}/pipeline?ref={Uri.EscapeDataString(createOptions.Ref)}{variables}";
    }

    public Pipeline CreatePipelineWithTrigger(string token, string @ref, Dictionary<string, string> variables)
    {
        var variablesToAdd = new StringBuilder();
        foreach (var variable in variables)
        {
            variablesToAdd.Append("&variables[").Append(variable.Key).Append("]=").Append(Uri.EscapeDataString(variable.Value));
        }

        return _api.Post().To<Pipeline>($"{_projectPath}/trigger/pipeline?token={token}&ref={@ref}{variablesToAdd}");
    }

    public IEnumerable<PipelineBasic> Search(PipelineQuery query)
    {
        var url = CreateSearchUrl(query);
        return _api.Get().GetAll<PipelineBasic>(url);
    }

    public GitLabCollectionResponse<PipelineBasic> SearchAsync(PipelineQuery query)
    {
        var url = CreateSearchUrl(query);
        return _api.Get().GetAllAsync<PipelineBasic>(url);
    }

    private string CreateSearchUrl(PipelineQuery query)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        var queryEntries = new Dictionary<string, string>(StringComparer.Ordinal);
        if (query.Scope.HasValue)
            queryEntries.Add("scope", query.Scope.Value.ToString());
        if (query.Status.HasValue)
            queryEntries.Add("status", query.Status.Value.ToString().ToLowerInvariant());
        if (!string.IsNullOrWhiteSpace(query.Ref))
            queryEntries.Add("ref", query.Ref);
        if (!string.IsNullOrWhiteSpace(query.Sha))
            queryEntries.Add("sha", query.Sha);
        if (query.YamlErrors.HasValue)
            queryEntries.Add("yaml_errors", query.YamlErrors.Value.ToString());
        if (!string.IsNullOrWhiteSpace(query.Name))
            queryEntries.Add("name", query.Name);
        if (!string.IsNullOrWhiteSpace(query.Username))
            queryEntries.Add("username", query.Username);
        if (query.UpdatedAfter.HasValue)
            queryEntries.Add("updated_after", query.UpdatedAfter.Value.ToString("O"));
        if (query.UpdatedBefore.HasValue)
            queryEntries.Add("updated_before", query.UpdatedBefore.Value.ToString("O"));
        if (query.OrderBy.HasValue)
            queryEntries.Add("order_by", query.OrderBy.Value.ToString());
        if (query.Sort.HasValue)
            queryEntries.Add("sort", query.Sort.Value.ToString());
        if (query.PerPage.HasValue)
            queryEntries.Add("per_page", query.PerPage.Value.ToString());

        var stringQuery = string.Join("&", queryEntries.Select(kp => $"{kp.Key}={kp.Value}"));
        var url = $"{_projectPath}/pipelines{(queryEntries.Count != 0 ? $"?{stringQuery}" : string.Empty)}";
        return url;
    }

    public void Delete(long pipelineId)
    {
        _api.Delete().Execute($"{_pipelinesPath}/{pipelineId.ToStringInvariant()}");
    }

    public IEnumerable<PipelineVariable> GetVariables(long pipelineId)
    {
        return _api.Get().GetAll<PipelineVariable>($"{_projectPath}/pipelines/{pipelineId.ToStringInvariant()}/variables");
    }

    public GitLabCollectionResponse<PipelineVariable> GetVariablesAsync(long pipelineId)
    {
        return _api.Get().GetAllAsync<PipelineVariable>($"{_projectPath}/pipelines/{pipelineId.ToStringInvariant()}/variables");
    }

    public TestReport GetTestReports(long pipelineId)
    {
        return _api.Get().To<TestReport>($"{_projectPath}/pipelines/{pipelineId.ToStringInvariant()}/test_report");
    }

    public TestReportSummary GetTestReportsSummary(long pipelineId)
    {
        return _api.Get().To<TestReportSummary>($"{_projectPath}/pipelines/{pipelineId.ToStringInvariant()}/test_report_summary");
    }

    public GitLabCollectionResponse<Bridge> GetBridgesAsync(PipelineBridgeQuery query)
    {
        var url = CreateGetBridgesUrl(query);
        return _api.Get().GetAllAsync<Bridge>(url);
    }

    private string CreateGetBridgesUrl(PipelineBridgeQuery query)
    {
        var url = $"{_pipelinesPath}/{query.PipelineId.ToStringInvariant()}/bridges";
        url = Utils.AddArrayParameter(url, "scope", query.Scope);
        return url;
    }

    public Task<Pipeline> RetryAsync(long pipelineId, CancellationToken cancellationToken = default)
    {
        var url = $"{_pipelinesPath}/{pipelineId.ToStringInvariant()}/retry";
        return _api.Post().ToAsync<Pipeline>(url, cancellationToken);
    }

    public Task<Pipeline> UpdateMetadataAsync(long pipelineId, PipelineMetadataUpdate update, CancellationToken cancellationToken = default)
    {
        var updatedMetadataValues = new Dictionary<string, string>(StringComparer.Ordinal);

        if (update.Name is not null)
        {
            updatedMetadataValues.Add("name", update.Name);
        }

        return _api.Put().With(new UrlEncodedContent(updatedMetadataValues)).ToAsync<Pipeline>($"{_pipelinesPath}/{pipelineId.ToStringInvariant()}/metadata", cancellationToken);
    }
}
