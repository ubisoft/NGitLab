using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface ITagClient
    {
        Tag Create(TagCreate tag);
        void Delete(string name);
        IEnumerable<Tag> All { get; }
        RealeaseInfo CreateRelease(string name, ReleaseCreate data);
        RealeaseInfo UpdateRelease(string name, ReleaseUpdate data);
    }
}
