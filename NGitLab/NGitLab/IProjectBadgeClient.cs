using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IProjectBadgeClient
    {
        IEnumerable<Badge> All { get; }
        Badge this[int id] { get; }
        Badge Create(BadgeCreate badge);
        Badge Update(int id, BadgeUpdate badge);
        void Delete(int id);
    }
}
