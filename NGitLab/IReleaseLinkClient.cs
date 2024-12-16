using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface IReleaseLinkClient
{
    IEnumerable<ReleaseLink> All { get; }

    ReleaseLink this[long id] { get; }

    ReleaseLink Create(ReleaseLinkCreate data);

    ReleaseLink Update(long id, ReleaseLinkUpdate data);

    void Delete(long id);
}
