using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

/// <summary>
/// https://github.com/gitlabhq/gitlabhq/blob/master/doc/api/lint.md
/// </summary>
public interface ILintClient
{
    Task<LintCI> ValidateCIYamlContentAsync(string projectId, string yamlContent, LintCIOptions options, CancellationToken cancellationToken = default);

    Task<LintCI> ValidateProjectCIConfigurationAsync(string projectId, LintCIOptions options, CancellationToken cancellationToken = default);
}
