using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

public interface ITagClient
{
    Tag Create(TagCreate tag);

    Task<Tag> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    void Delete(string name);

    IEnumerable<Tag> All { get; }

    GitLabCollectionResponse<Tag> GetAsync(TagQuery query);
}
