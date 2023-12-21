using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal class LintClient : ClientBase, ILintClient
{
    public LintClient(ClientContext context)
        : base(context)
    {
    }

    public Task<Models.LintCI> ValidateCIYamlContentAsync(string projectId, string yamlContent, LintCIOptions options, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Models.LintCI> ValidateProjectCIConfigurationAsync(string projectId, LintCIOptions options, CancellationToken cancellationToken = default)
    {
        await Task.Yield();

        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.View);
            var @ref = string.IsNullOrEmpty(options.Ref) ? project.DefaultBranch : options.Ref;

            var lintCi = project.LintCIs.FirstOrDefault(ci =>
            {
                return string.Equals(ci.Ref, @ref, StringComparison.Ordinal);
            }) ?? new LintCI(@ref, valid: false, "Reference not found");

            return new Models.LintCI
            {
                Valid = lintCi.Valid,
                Errors = lintCi.Errors,
            };
        }
    }
}
