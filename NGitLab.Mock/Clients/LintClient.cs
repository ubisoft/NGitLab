using System;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal class LintClient : ILintClient
    {
        private readonly ClientContext _context;

        public LintClient(ClientContext context)
        {
            _context = context;
        }

        public Task<LintCI> ValidateCIYamlContentAsync(string projectId, string yamlContent, LintCIOptions options, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<LintCI> ValidateProjectCIConfigurationAsync(string projectId, LintCIOptions options, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
