using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class PipelineClient : IPipelineClient
    {
        private readonly API _api;
        private readonly string _projectPath;
        private readonly string _pipelinesPath;

        public PipelineClient(API api, int projectId)
        {
            _api = api;
            _projectPath = $"{Project.Url}/{projectId}";
            _pipelinesPath = $"{Project.Url}/{projectId}/pipelines";
        }

        public IEnumerable<PipelineBasic> All => _api.Get().GetAll<PipelineBasic>(_pipelinesPath);

        public IEnumerable<Job> AllJobs => _api.Get().GetAll<Job>($"{_projectPath}/jobs");

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

        public Pipeline this[int id] => _api.Get().To<Pipeline>($"{_pipelinesPath}/{id}");

        public Job[] GetJobs(int pipelineId)
        {
            // For a reason gitlab returns the jobs in the reverse order as
            // they appear in their UI. Here we reverse them!
            var jobs = _api.Get().GetAll<Job>($"{_pipelinesPath}/{pipelineId}/jobs").Reverse().ToArray();
            return jobs;
        }

        public Pipeline Create(string @ref)
        {
            return _api.Post().To<Pipeline>($"{_projectPath}/pipeline?ref={@ref}");
        }

        public Pipeline Create(PipelineCreate createOptions)
        {
            var variables = new StringBuilder();
            foreach (var variable in createOptions.Variables)
            {
                // see https://docs.gitlab.com/ee/api/#array-of-hashes
                variables
                    .Append("&variables[][key]=").Append(Uri.EscapeDataString(variable.Key))
                    .Append("&variables[][value]=").Append(Uri.EscapeDataString(variable.Value));
            }

            return _api.Post().To<Pipeline>($"{_projectPath}/pipeline?ref={Uri.EscapeDataString(createOptions.Ref)}{variables}");
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
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var queryEntries = new Dictionary<string, string>(StringComparer.Ordinal);
            if (query.Scope.HasValue)
                queryEntries.Add("scope", query.Scope.Value.ToString());
            if (query.Status.HasValue)
                queryEntries.Add("status", query.Status.Value.ToString());
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

            var stringQuery = string.Join("&", queryEntries.Select(kp => $"{kp.Key}={kp.Value}"));
            return _api.Get().GetAll<PipelineBasic>($"{_projectPath}/pipelines{(queryEntries.Any() ? $"?{stringQuery}" : string.Empty)}");
        }

        public void Delete(int pipelineId)
        {
            _api.Delete().Execute($"{_pipelinesPath}/{pipelineId}");
        }

        public IEnumerable<PipelineVariable> GetVariables(int pipelineId)
        {
            return _api.Get().GetAll<PipelineVariable>($"{_projectPath}/pipelines/{pipelineId}/variables");
        }

        public IEnumerable<TestReports> GetTestReports(int pipelineId)
        {
            return _api.Get().GetAll<TestReports>($"{_projectPath}/pipelines/{pipelineId}/test_report");
        }
    }
}
