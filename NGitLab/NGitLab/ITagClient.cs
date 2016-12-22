using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface ITagClient : IEnumerable<Tag>
    {
        Tag Create(TagCreate tag);
        void Delete(string name);
    }
}