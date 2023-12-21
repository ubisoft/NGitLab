using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

public interface IGlobalJobClient
{
    Task<Job> GetJobFromJobTokenAsync(string token, CancellationToken cancellationToken = default);
}
