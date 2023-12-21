using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

public interface IProtectedTagClient
{
    ProtectedTag ProtectTag(TagProtect protect);

    Task<ProtectedTag> ProtectTagAsync(TagProtect protect, CancellationToken cancellationToken = default);

    void UnprotectTag(string name);

    Task UnprotectTagAsync(string name, CancellationToken cancellationToken = default);

    ProtectedTag GetProtectedTag(string name);

    Task<ProtectedTag> GetProtectedTagAsync(string name, CancellationToken cancellationToken = default);

    IEnumerable<ProtectedTag> GetProtectedTags();

    GitLabCollectionResponse<ProtectedTag> GetProtectedTagsAsync();
}
