using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Impl
{
    internal class LintClient : ILintClient
    {
        private readonly API _api;

        public LintClient(API api)
        {
            _api = api;
        }

        public Task<LintCI> LintGitLabCIYamlAsync(string projectId, string yamlContent, LintCIOptions options, CancellationToken cancellationToken = default)
        {
            var url = BuildLintCIUrl(projectId, options);
            url = Utils.AddParameter(url, "content", yamlContent);

            return _api.Post().ToAsync<LintCI>(url, cancellationToken);
        }

        public Task<LintCI> LintProjectGitLabCIYamlAsync(string projectId, LintCIOptions options, CancellationToken cancellationToken = default)
        {
            var url = BuildLintCIUrl(projectId, options);

            return _api.Get().ToAsync<LintCI>(url, cancellationToken);
        }

        private static string BuildLintCIUrl(string projectId, LintCIOptions options)
        {
            var url = Project.Url + "/" + WebUtility.UrlEncode(projectId) + LintCI.Url;

            url = Utils.AddParameter(url, "dry_run", options.DryRun);
            url = Utils.AddParameter(url, "ref", options.Ref);
            url = Utils.AddParameter(url, "include_jobs", options.IncludeJobs);

            return url;
        }
    }
}
