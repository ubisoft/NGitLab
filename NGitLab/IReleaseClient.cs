using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab
{
    public interface IReleaseClient
    {
        IEnumerable<ReleaseInfo> All { get; }

        ReleaseInfo this[string tagName] { get; }

        ReleaseInfo Create(ReleaseCreate data);

        Task<ReleaseInfo> CreateAsync(ReleaseCreate data, CancellationToken cancellationToken = default);

        ReleaseInfo Update(ReleaseUpdate data);

        void Delete(string tagName);

        IReleaseLinkClient ReleaseLinks(string tagName);
    }
}
