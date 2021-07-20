using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IReleaseLinkClient
    {
        IEnumerable<ReleaseLink> All { get; }

        ReleaseLink this[int id] { get; }

        ReleaseLink Create(ReleaseLinkCreate data);

        ReleaseLink Update(int id, ReleaseLinkUpdate data);

        void Delete(int id);
    }
}
